using ICMA.Filters;
using ICMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    public class RFPController : BaseController
    {
        private readonly IRfpRequestFacade _rfpRequestFacade;
        private readonly IRfpAttachmentFacade _rfpAttachmentFacade;
        private readonly IScoreRuleFacade _rfpScoreRuleFacade;
        private readonly IProductTypeFacade _prodTypeFacade;

        #region CONSTRUCTORS

        public RFPController(IRfpRequestFacade rfpRequestFacade, IRfpAttachmentFacade rfpAttachmentFacade, IScoreRuleFacade rfpScoreRuleFacade, IProductTypeFacade prodTypeFacade)
        {
            _rfpRequestFacade = rfpRequestFacade ?? throw new ArgumentNullException(nameof(rfpRequestFacade));
            _rfpAttachmentFacade = rfpAttachmentFacade ?? throw new ArgumentNullException(nameof(rfpAttachmentFacade));
            _rfpScoreRuleFacade = rfpScoreRuleFacade ?? throw new ArgumentNullException(nameof(rfpScoreRuleFacade));
            _prodTypeFacade = prodTypeFacade ?? throw new ArgumentNullException(nameof(prodTypeFacade));
        }
        #endregion

        // GET: RFP
        [RoleAuthorize("Vendor,RFP")]
        public ActionResult Index(string rfp_key)
        {
            if(string.IsNullOrWhiteSpace(rfp_key))
            {
                var response = _rfpRequestFacade.GetWhere($"co_organization.org_key='{CurrentUser.User.org_key}'");

                if(response != null && response.Count > 0)
                {
                    response.ForEach(p => p.RACIContactList = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, rfp_key = p.rfp_key.ToString() }));
                    response.ForEach(p => p.ScoreRules = _rfpScoreRuleFacade.GetWhere($"rfp_score_rules.eva_rfp_key='{p.rfp_key.ToString()}'"));
                }

                return View(response);
            }
            else
            {
                var response = _rfpRequestFacade.Find(rfp_key);

                if (response.Success)
                {
                    response.Result.RACIContactList = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, rfp_key = response.Result.rfp_key.ToString() });

                    if (CurrentUser.User.org_key.ToString() != response.Result.rfp_org_key.ToString())
                    {
                        _miscFacade.ViewPage(new CoPageView
                        {
                            pv_key = Guid.NewGuid(),
                            pv_rfp_key = Guid.Parse(rfp_key),
                            pv_prd_key = null,
                            pv_source_ct_key = CurrentUser.User.ct_key,
                            pv_add_user = CurrentUser.User.ct_key.ToString(),
                            pv_view_datetime = DateTime.Now
                        });
                    }
                    
                    TempData["Files"] = _rfpAttachmentFacade.GetWhere($"rfa_key in (select axr_rfa_key from attachment_x_request where axr_rfp_key='{rfp_key}')");
                    return View("ViewRFP", response.Result);
                }
                else
                {
                    return View("ViewRFP");
                }
            }
        }

        public ActionResult ViewRFP(string rfp_key)
        {
            var request = _rfpRequestFacade.Find(rfp_key);

            if (request.Success)
            {
                TempData["Files"] = _rfpAttachmentFacade.GetWhere($"rfa_key in (select axr_rfa_key from attachment_x_request where axr_rfp_key='{rfp_key}')");
                return View(request.Result);
            }
            else
            {
                return View();
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Edit(string rfp_key)
        {
            var request = _rfpRequestFacade.Find(rfp_key);

            if (request.Success)
            {
                ViewData["Categories"] = _prodTypeFacade.GetAll();
                request.Result.RACIContactList = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, rfp_key = request.Result.rfp_key.ToString() });

                var files = _rfpAttachmentFacade.GetWhere($"rfa_key in (select axr_rfa_key from attachment_x_request where axr_rfp_key='{rfp_key}')");

                if(files != null && files.Where(p=> p.rfa_type == "QA").Count() > 0)
                {
                    request.Result.QuestionnaireFiles = string.Join(",", files.Where(p => p.rfa_type == "QA").Select(p => p.rfa_key));
                }

                if (files != null && files.Where(p => p.rfa_type == "AA").Count() > 0)
                {
                    request.Result.RfpAttachments = string.Join(",", files.Where(p => p.rfa_type == "AA").Select(p => p.rfa_key));
                }

                return View(request.Result);
            }
            else
            {
                return View();
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Create()
        {
            ViewData["Categories"] = _prodTypeFacade.GetAll();
            return View(new RfpRequest() { rfp_key = Guid.NewGuid() });
        }

        [RoleAuthorize("RFP")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(RfpRequest entity)
        {
            ViewData["Categories"] = _prodTypeFacade.GetAll();
            var resp_contact = entity.RACIContactList.FirstOrDefault(p => p.lk5_key.ToString() == "5592b25d-d98a-4914-9c8e-ac28d6c11116");

            if (resp_contact == null)
            {
                ModelState.AddModelError("", "Please add team member with 'Responsible' role.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    entity.rfp_prd_key = null;
                    entity.rfp_org_key = CurrentUser.User.org_key;
                    var result = _rfpRequestFacade.Add(entity, CurrentUser.User.ct_key.ToString());

                    if (result.Success)
                    {
                        var file_list = new List<string>();
                        if (!string.IsNullOrWhiteSpace(entity.QuestionnaireFiles))
                            file_list.AddRange(entity.QuestionnaireFiles.Split(','));

                        foreach(string rfa_key in file_list)
                        {
                            _rfpAttachmentFacade.UpdateField(rfa_key, "rfa_rfp_key", entity.rfp_key.ToString(), string.Empty);
                            _rfpAttachmentFacade.UpdateField(rfa_key, "rfa_type", "QA", string.Empty);
                            _rfpAttachmentFacade.UpdateField(rfa_key, "delete_flag", "0", string.Empty);
                            _rfpAttachmentFacade.Attach_RfpRequest_File(rfa_key, entity.rfp_key.ToString());
                        }

                        file_list.Clear();

                        if (!string.IsNullOrWhiteSpace(entity.RfpAttachments))
                            file_list.AddRange(entity.RfpAttachments.Split(','));

                        foreach (string rfa_key in file_list)
                        {
                            _rfpAttachmentFacade.UpdateField(rfa_key, "rfa_rfp_key", entity.rfp_key.ToString(), string.Empty);
                            _rfpAttachmentFacade.UpdateField(rfa_key, "rfa_type", "AA", string.Empty);
                            _rfpAttachmentFacade.UpdateField(rfa_key, "delete_flag", "0", string.Empty);
                            _rfpAttachmentFacade.Attach_RfpRequest_File(rfa_key, entity.rfp_key.ToString());
                        }

                        // Save added contacts to RACI.
                        foreach (var contact in entity.RACIContactList)
                        {
                            _contactFacade.Add_RACI_Contact(contact.lk5_key.ToString(), contact.ct_key.ToString(), entity.rfp_key.ToString(), null, CurrentUser.User.ct_key.ToString());
                        }

                        return RedirectToAction("", "rfp");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Could not save RFP");
                    }
                }

                return View(entity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(entity);
            }
        }

        [RoleAuthorize("RFP")]
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Update(string rfp_key, string key, string value)
        {
            value = string.IsNullOrWhiteSpace(value) ? "" : Regex.Replace(value, @"\t|\n|\r", "");
            var response = new AppResponseModel<int>();

            try
            {
                var resp = _rfpRequestFacade.UpdateData(new DataObject {
                    primary_key_name = "rfp_key",
                    primary_key_value = rfp_key,
                    data_field_name = key,
                    data_field_value = value.Trim(),
                    data_type = typeof(String),
                    user_name = string.Empty,
                    table_name = "rfp_rerquest"
                });

                if (resp > 0)
                {
                    response.IsSuccess = true;
                    response.Data = resp;
                    response.Message = Messages.Updated;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.NoUpdate;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Criteria(string rfp_key)
        {
            try
            {
                var request = _rfpRequestFacade.Find(rfp_key);

                if (request.Success)
                {
                    request.Result.ScoreRules = _rfpScoreRuleFacade.GetWhere($"rfp_score_rules.eva_rfp_key='{rfp_key}'");
                    return View(request.Result);
                }
                else
                {
                    return View(new RfpRequest());
                }
            }
            catch(Exception ex)
            {
                return View(new RfpRequest());
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Score(string rfp_key)
        {
            try
            {
                var request = _rfpRequestFacade.Find(rfp_key);

                if (request.Success)
                {
                    request.Result.ScoreRules = _rfpScoreRuleFacade.GetWhere($"rfp_score_rules.eva_rfp_key='{rfp_key}'");
                    return View(request.Result);
                }
                else
                {
                    return View(new RfpRequest());
                }
            }
            catch (Exception ex)
            {
                return View(new RfpRequest());
            }
        }


        [RoleAuthorize("RFP")]
        public ActionResult SaveCriteria(RfpScoreRule score_rule)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                if(score_rule.eva_key == null)
                {
                    score_rule.eva_key = Guid.NewGuid();

                    var result = _rfpScoreRuleFacade.Add(score_rule, CurrentUser.User.ct_key.ToString());

                    if (result.Success)
                    {
                        response.IsSuccess = true;
                        response.Data = true;
                        response.Message = "Score rule saved successfully!";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var score_list = _rfpScoreRuleFacade.GetWhere($"rfp_score_rules.eva_key='{score_rule.eva_key.ToString()}'");

                    if(score_list != null && score_list.Count > 0 && score_list[0].eva_key == score_rule.eva_key)
                    {
                        RfpScoreRule temp_score_rule = score_list[0];
                        temp_score_rule.eva_change_user = CurrentUser.User.ct_key.ToString();
                        temp_score_rule.eva_criteria_name = score_rule.eva_criteria_name;
                        temp_score_rule.eva_criteria_description = score_rule.eva_criteria_description;
                        temp_score_rule.eva_point_max = score_rule.eva_point_max;
                        temp_score_rule.eva_weight = score_rule.eva_weight;

                        var result = _rfpScoreRuleFacade.Update(temp_score_rule, CurrentUser.User.ct_key.ToString());

                        if(result.Success)
                        {
                            response.IsSuccess = true;
                            response.Data = true;
                            response.Message = "Score rule saved successfully!";
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                response.IsSuccess = false;
                response.Message = "Could not saved Score rule!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Templates(string rfp_key = "")
        {
            try
            {
                if (string.IsNullOrEmpty(rfp_key))
                {
                    return View("Templates-Create", new RfpRequest());
                }
                else
                {
                    var request = _rfpRequestFacade.Find(rfp_key);

                    if (request.Success)
                    {
                        return View(request.Result);
                    }
                    else
                    {
                        return View("Templates-Create", new RfpRequest());
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Templates-Create", new RfpRequest());
            }
        }

        [RoleAuthorize("RFP")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Templates(RfpRequest entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    entity.rfp_prd_key = null;
                    entity.rfp_key = Guid.NewGuid();
                    entity.rfp_org_key = CurrentUser.User.org_key;
                    var result = _rfpRequestFacade.Add(entity, CurrentUser.User.ct_key.ToString());

                    if (result.Success)
                    {
                        return RedirectToAction("Templates", "RFP", new { rfp_key = entity.rfp_key.ToString() });
                    }
                }

                return View("Templates-Create", entity);
            }
            catch (Exception ex)
            {
                return View("Templates-Create", entity);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Delete(string id)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var result = _rfpRequestFacade.Remove(id, CurrentUser.User.ct_key.ToString());

                if (result)
                {
                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = "RFP deleted successfully!";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.Failed;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult RemoveScoreRule(string id)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var result = _rfpScoreRuleFacade.Remove(id, string.Empty);

                if (result)
                {
                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = "Score rule deleted successfully!";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Could not remove score rule.";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Remove_File(string rfa_key)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var result = _rfpAttachmentFacade.Remove(rfa_key, CurrentUser.User.ct_key.ToString());

                if (result)
                {
                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = "File deleted successfully!";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.Failed;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Attach_File(string rfp_key, string rfa_key, string file_type)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                _rfpAttachmentFacade.UpdateField(rfa_key, "rfa_rfp_key", rfp_key, CurrentUser.User.ct_key.ToString());
                _rfpAttachmentFacade.UpdateField(rfa_key, "rfa_type", file_type, CurrentUser.User.ct_key.ToString());
                _rfpAttachmentFacade.UpdateField(rfa_key, "delete_flag", "0", CurrentUser.User.ct_key.ToString());
                _rfpAttachmentFacade.Attach_RfpRequest_File(rfa_key, rfp_key);

                response.IsSuccess = true;
                response.Data = true;
                response.Message = "File saved successfully!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Attach_RACI(string rfp_key, string ct_key, string lk5_key)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                _contactFacade.Add_RACI_Contact(lk5_key, ct_key, rfp_key, null, CurrentUser.User.ct_key.ToString());
                response.IsSuccess = true;
                response.Data = true;
                response.Message = "RACI contact saved successfully!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [RoleAuthorize("RFP")]
        public ActionResult Remove_RACI(string rfp_key, string ct_key)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                _contactFacade.Delete_RACI_Contact(ct_key, rfp_key, null);
                response.IsSuccess = true;
                response.Data = true;
                response.Message = "RACI contact removed successfully!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}