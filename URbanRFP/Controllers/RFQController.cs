using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    public class RFQController : BaseController
    {
        private readonly ISearchFacade _searchFacade;
        private readonly IProductFacade _productFacade;
        private readonly IRfpRequestFacade _rfpFacade;
        private readonly IRfpAttachmentFacade _rfpAttachmentFacade;

        #region CONSTRUCTORS

        public RFQController(ISearchFacade searchFacade, IProductFacade productFacade, IRfpRequestFacade rfpFacade, IRfpAttachmentFacade rfpAttachmentFacade)
        {
            _productFacade = productFacade ?? throw new ArgumentNullException(nameof(productFacade));
            _searchFacade = searchFacade ?? throw new ArgumentNullException(nameof(searchFacade));
            _rfpFacade = rfpFacade ?? throw new ArgumentNullException(nameof(rfpFacade));
            _rfpAttachmentFacade = rfpAttachmentFacade ?? throw new ArgumentNullException(nameof(rfpAttachmentFacade));
        }
        #endregion

        // GET: RFQ
        public ActionResult Index(string rfp_key)
        {
            var request = _rfpFacade.Find(rfp_key);

            if (request.Success)
            {
                ViewData["Product"] = _searchFacade.FindRfpProduct(request.Result.rfp_prd_key.ToString());
                return View(request.Result);
            }
            else
            {
                ViewData["Product"] = new ProductOrganizationViewModel();
                return View(new RfpRequest());
            }
        }

        public ActionResult Edit(string prdkey)
        {
            ViewData["Product"] = _searchFacade.FindRfpProduct(prdkey);
            var rfq = _rfpFacade.GetRFQDetails(CurrentUser.User.org_key.ToString(), prdkey);

            if (rfq == null)
            {
                rfq = new RfpRequest() { rfp_key = Guid.NewGuid() };
            }
            else
            {
                rfq.RACIContactList = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, rfp_key = rfq.rfp_key.ToString() });

                var files = _rfpAttachmentFacade.GetWhere($"rfa_key in (select axr_rfa_key from attachment_x_request where axr_rfp_key='{rfq.rfp_key.ToString()}')");

                if (files != null && files.Where(p => p.rfa_type == "QA").Count() > 0)
                {
                    rfq.QuestionnaireFiles = string.Join(",", files.Where(p => p.rfa_type == "QA").Select(p => p.rfa_key));
                }

                if (files != null && files.Where(p => p.rfa_type == "AA").Count() > 0)
                {
                    rfq.RfpAttachments = string.Join(",", files.Where(p => p.rfa_type == "AA").Select(p => p.rfa_key));
                }
            }
            
            return View(rfq);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(RfpRequest entity, string prdkey)
        {
            ViewData["Product"] = _searchFacade.FindRfpProduct(prdkey);
            if (entity.RACIContactList == null) entity.RACIContactList = new List<ContactRACI>();
            List<ContactRACI> existing_raci_contacts = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, rfp_key = entity.rfp_key.ToString() });

            var resp_contact = entity.RACIContactList.FirstOrDefault(p => p.lk5_key.ToString() == "5592b25d-d98a-4914-9c8e-ac28d6c11116");

            if (resp_contact == null)
            {
                ModelState.AddModelError("", "Please add team member with 'Responsible' role.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    entity.rfp_org_key = CurrentUser.User.org_key;
                    entity.rfp_prd_key = Guid.Parse(prdkey);

                    var current_rfp_resp = _rfpFacade.GetRFQDetails(CurrentUser.User.org_key.ToString(), prdkey);
                    Response<RfpRequest> result;

                    if (current_rfp_resp != null)
                    {
                        result = _rfpFacade.Update(entity, CurrentUser.User.ct_key.ToString());
                    }
                    else
                    {
                        result = _rfpFacade.Add(entity, CurrentUser.User.ct_key.ToString());
                    }

                    if (result.Success)
                    {

                        var file_list = new List<string>();
                        if (!string.IsNullOrWhiteSpace(entity.QuestionnaireFiles))
                            file_list.AddRange(entity.QuestionnaireFiles.Split(','));

                        foreach (string rfa_key in file_list)
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

                        // Delete removed contacts from RACI.
                        if(existing_raci_contacts != null && existing_raci_contacts.Count > 0)
                        {
                            foreach(var item in existing_raci_contacts)
                            {
                                _contactFacade.Delete_RACI_Contact(item.ct_key.ToString(), entity.rfp_key.ToString(), null);
                            }
                        }

                        // Save added contacts to RACI.
                        foreach (var contact in entity.RACIContactList)
                        {
                            //_contactFacade.Attach_Contact_Request(resp.Result.ct_key.ToString(), entity.rfp_key.ToString(), contact.role_id, contact.title, string.Empty);
                            _contactFacade.Add_RACI_Contact(contact.lk5_key.ToString(), contact.ct_key.ToString(), entity.rfp_key.ToString(), null, CurrentUser.User.ct_key.ToString());
                        }

                        // Save Request Respond Invitation
                        existing_raci_contacts = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, rfp_key = entity.rfp_key.ToString() });

                        if(existing_raci_contacts != null && existing_raci_contacts.Count > 0)
                        {
                            var responsible_contact = existing_raci_contacts.FirstOrDefault(p => p.lk5_key.ToString() == "5592b25d-d98a-4914-9c8e-ac28d6c11116");

                            if(responsible_contact != null)
                            {
                                var invitation = new RfpRespondInvitation
                                {
                                    inv_add_user = CurrentUser.User.ct_key.ToString(),
                                    inv_change_user = CurrentUser.User.ct_key.ToString(),
                                    inv_ct_key = responsible_contact.ct_key,
                                    inv_email = responsible_contact.ct_email,
                                    inv_first_name = responsible_contact.ct_first_name,
                                    inv_last_name = responsible_contact.ct_last_name,
                                    inv_full_name = $"{responsible_contact.ct_first_name} {responsible_contact.ct_last_name}",
                                    inv_key = Guid.NewGuid(),
                                    inv_rfp_key = entity.rfp_key
                                };

                                _rfpFacade.SaveRfpRespondInvitation(invitation);
                            }
                        }
                        
                        return RedirectToAction("", "dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Could not save RFQ");
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
    }
}