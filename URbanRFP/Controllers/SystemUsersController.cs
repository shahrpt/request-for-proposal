using ICMA.Filters;
using ICMA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Helpers;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    public class SystemUsersController : BaseController
    {

        private readonly IContactFacade contactService;
        private readonly IOrganizationFacade orgService;

        private UrbanRFPEntities db = new UrbanRFPEntities();
        #region CONSTRUCTORS

        public SystemUsersController(IContactFacade _contactService, IOrganizationFacade _orgService)
        {
            this.contactService = _contactService ?? throw new ArgumentNullException(nameof(_contactService));
            this.orgService = _orgService ?? throw new ArgumentNullException(nameof(_orgService));

        }

        #endregion


        [RoleAuthorize("Admin")]
        public async Task<ActionResult> Index()
        {
            ViewBag.UserCount = await db.co_contact.CountAsync();
            return View();
        }

        public async Task<PartialViewResult> _List()
        {
            //UrbanRFP.Facade.Interfaces.

            byte flag = 1;

            var users = await db.co_contact.Where(w => w.ct_delete_flag != flag).ToListAsync();
            return PartialView(users);
        }

        // GET: SystemUsers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SystemUsers/Create
        public async Task<PartialViewResult> Create(string id)
        {
            ViewBag.OrgName = "";
            ViewBag.OrgDomain = "";
            ViewBag.Perm = "";
            if (!String.IsNullOrEmpty(id))
            {
                ViewBag.CID = id;
                var user = await db.co_contact.Where(w => w.ct_key.ToString() == id).FirstOrDefaultAsync();
                if (user == null)
                    throw new Exception("User Not Fount");
                var org = await db.contact_x_organization.Where(w => w.cxo_ct_key.ToString() == id).FirstOrDefaultAsync();
                if (org != null)
                {
                    ViewBag.OrgName = org.co_organization.org_legal_name;
                    ViewBag.OrgDomain = org.co_organization.org_domain;
                }
                var Perm = await db.co_permission.Where(w => w.per_ct_key.ToString() == id).FirstOrDefaultAsync();
                if (Perm != null)
                {
                    ViewBag.Perm = Perm.per_type;
                }
                return PartialView(user);
            }
            else
            {

                co_contact model = new co_contact();

                return PartialView(model);
            }

        }


        public async Task<JsonResult> getEmailData(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return Json(new
                {
                    status = "empty"
                }, JsonRequestBehavior.AllowGet);
            }
            var org = await db.co_organization.Where(w => 
            email.ToLower().Trim().Contains(w.org_domain.ToLower().Trim())).FirstOrDefaultAsync();
            if (org != null)
            {
                return Json(new
                {
                    status = "org",
                    OrgName = org.org_legal_name,
                    OrgDomain = org.org_domain,
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = "no-org"
            }, JsonRequestBehavior.AllowGet);
        }
        //
        [HttpPost]
        public async Task<JsonResult> setActive(string key, bool status)
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            var user = await db.co_contact.Where(w => w.ct_key.ToString() == key).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("User Not Fount");
            user.ct_active = status ? (byte)1 : (byte)0;
            user.ct_change_date = DateTime.Now;
            user.ct_change_user = currUser;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(new
            {
                status = "Done"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> removeUser(string id)
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            var user = await db.co_contact.Where(w => w.ct_key.ToString() == id).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("User Not Fount");
            user.ct_delete_flag = (byte)1;
            user.ct_change_date = DateTime.Now;
            user.ct_change_user = currUser;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(new
            {
                status = "Done"
            }, JsonRequestBehavior.AllowGet);
        }

       [HttpPost]
        public async Task<JsonResult> SaveUser(co_contact data, bool IsActive, string ConfirmPassword,
           string upID, string OrgName, string OrgDomain, string Permission)
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            Guid recID = new Guid();
            try
            {
                if (data.ct_password != ConfirmPassword)
                    throw new Exception("Password Must be Matching.");

                if (String.IsNullOrWhiteSpace(upID))
                {
                    var existingUser = await db.co_contact.Where(w => 
                    w.ct_email.ToLower().Trim() == data.ct_email.ToLower().Trim()).FirstOrDefaultAsync();
                    if (existingUser != null)
                    {
                        return Json(new
                        {
                            status = "error",
                            msg = "This email is already registered"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    data.ct_active = IsActive ? (byte)1 : (byte)0;
                    data.ct_password = data.ct_password.GetEncryptedString();
                    data.ct_add_date =  DateTime.Now;
                    data.ct_add_user = currUser;
                    data.ct_key = Guid.NewGuid();
                    db.co_contact.Add(data);
                    await db.SaveChangesAsync();
                    recID = data.ct_key;
                }
                else
                {
                    var gID = Guid.Parse(upID);
                    var updata = await db.co_contact.Where(w => w.ct_key == gID).FirstOrDefaultAsync();
                    if (updata.ct_email != data.ct_email)
                    {
                        var existingUser = await db.co_contact.Where(w =>
                      w.ct_email.ToLower().Trim() == data.ct_email.ToLower().Trim()).FirstOrDefaultAsync();
                        if (existingUser != null)
                        {
                            return Json(new
                            {
                                status = "error",
                                msg = "This email is already registered"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    updata.ct_active = data.ct_active;
                    updata.ct_change_date = DateTime.Now;
                    updata.ct_change_user = currUser;
                    updata.ct_email = data.ct_email;
                    updata.ct_fax = data.ct_fax;
                    updata.ct_first_name = data.ct_first_name;
                    updata.ct_last_name = data.ct_last_name;
                    updata.ct_phone = data.ct_phone;
                    updata.ct_active = IsActive ? (byte)1 : (byte)0;
                    db.Entry(updata).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    recID = updata.ct_key;
                }
                if(!String.IsNullOrEmpty(OrgName) && !String.IsNullOrEmpty(OrgDomain) && String.IsNullOrWhiteSpace(upID))
                {
                    co_organization org = await db.co_organization.Where(w => w.org_legal_name.ToLower().Trim() == OrgName.ToLower().Trim()
                    && w.org_domain.ToLower().Trim() == OrgDomain.ToLower().Trim()).FirstOrDefaultAsync();

                    Guid orgKey = new Guid();
                    if (org == null)
                    {
                        var newOrg = new co_organization()
                        {
                            org_key = Guid.NewGuid(),
                            org_add_date = DateTime.Now,
                            org_add_user = currUser,
                            org_domain = OrgDomain,
                            org_legal_name = OrgName
                        };
                        db.co_organization.Add(newOrg);
                        await db.SaveChangesAsync();
                        orgKey = newOrg.org_key;
                    }
                    else
                        orgKey = org.org_key;

                    var con_org = await db.contact_x_organization.Where(w => w.cxo_org_key == orgKey).FirstOrDefaultAsync();
                    if (con_org == null)
                    {
                        var newOrgCon = new contact_x_organization()
                        {
                            cxo_key = Guid.NewGuid(),
                            cxo_add_date = DateTime.Now,
                            cxo_add_user = currUser,
                            cxo_ct_key = recID,
                            cxo_org_key = orgKey
                        };
                        db.contact_x_organization.Add(newOrgCon);
                        await db.SaveChangesAsync();
                    }
                }

                var perm = await db.co_permission.Where(w => w.per_ct_key == recID).FirstOrDefaultAsync();
                if (perm != null && Permission != perm.per_type)
                {
                    if (String.IsNullOrWhiteSpace(Permission))
                    {
                        db.co_permission.Remove(perm);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        perm.per_type = Permission;
                        perm.per_change_date = DateTime.Now;
                        perm.per_change_user = currUser;
                        db.Entry(perm).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                else if (perm == null)
                {
                    db.co_permission.Add(new co_permission
                    {
                        per_ct_key = recID,
                        per_key = Guid.NewGuid(),
                        per_add_date = DateTime.Now,
                        per_add_user = currUser,
                        per_type = Permission
                    });
                    await db.SaveChangesAsync();
                }
                
                return Json(new
                {
                    status = "Done"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                return Json(new
                {
                    status = "error",
                    msg = er.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult SaveUser_old(UserContactViewModel model, string upID)
        {

            string currUser = CurrentUser.User.ct_key.ToString();
            CoContact contact = new CoContact();
            if (String.IsNullOrEmpty(upID))
                contact = new CoContact()
                {
                    ct_email = model.Email,
                    ct_password = model.Password.GetEncryptedString(),
                    ct_first_name = model.FirstName,
                    ct_last_name = model.LastName,
                    ct_phone = model.Phone,
                    ct_fax = model.Fax,
                    ct_key = Guid.NewGuid(),
                    ct_add_user = currUser,
                    ct_activation_code = Guid.NewGuid(),
                    ct_active = model.IsActive? Convert.ToInt16(1) : Convert.ToInt16(0),
                };
            else
            {
                var up_user = db.co_contact.Where(w => w.ct_key.ToString() == upID).FirstOrDefault();
                contact = new CoContact()
                {
                    ct_email = model.Email,
                    ct_password = up_user.ct_password,
                    ct_first_name = model.FirstName,
                    ct_last_name = model.LastName,
                    ct_phone = model.Phone,
                    ct_fax = model.Fax,
                    ct_key = up_user.ct_key,
                    ct_id = up_user.ct_id,
                    ct_add_user = up_user.ct_add_user,
                    ct_change_user = currUser,
                    ct_change_date = DateTime.Now.ToString(),
                    ct_add_date = up_user.ct_add_date.HasValue ? up_user.ct_add_date.Value.ToString() : "",
                    ct_activation_code = up_user.ct_activation_code.HasValue ?
                    up_user.ct_activation_code.Value : Guid.NewGuid(),
                    ct_active = model.IsActive ? Convert.ToInt16(1) : Convert.ToInt16(0),
                };
            }
            try
            {


                var result = orgService.GetWhere($"org_domain='{model.OrgDomain.Trim()}'");

                if ((result == null || (result != null && result.Count == 0)) &&
                string.IsNullOrWhiteSpace(model.OrgName))
                {

                    return Json(new
                    {
                        status = "error",
                        msg = "Enter valid organization name."
                    }, JsonRequestBehavior.AllowGet);

                }

                var existingUser = contactService.GetByUsername(model.Email.Trim());

                if (existingUser.Success && String.IsNullOrEmpty(upID))
                {
                    return Json(new
                    {
                        status = "error",
                        msg = "This email is already registered."
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    CoOrganization org = null;

                    if (result.Count > 0)
                    {
                        org = result[0];
                    }
                    else
                    {
                        var org_resp = orgService.Add(new CoOrganization
                        {
                            org_add_date = DateTime.Now,
                            org_key = Guid.NewGuid(),
                            org_legal_name = model.OrgName.Trim(),
                            org_domain = model.OrgDomain,
                            org_add_user = currUser
                        }, currUser);

                        if (org_resp.Success)
                        {
                            org = org_resp.Result;
                            var response = contactService.Add(contact, currUser);

                            if (response.Success)
                            {
                                contactService.Add_Contact_Org(org.org_key.ToString(), contact.ct_key.ToString(), currUser);
                                // TempData["Message"] = "You have been registered successfully. Please check your email to activate account.";
                                // EmailManager.SendWelcome(contact.ct_email, string.Format("{0} {1}", contact.ct_first_name, contact.ct_last_name), contact.ct_activation_code.ToString());

                                return Json(new
                                {
                                    status = "Done",
                                    msg = "You Added User successfully."
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new
                                {
                                    status = "error",
                                    msg = "Could not register you. Please contact support."
                                }, JsonRequestBehavior.AllowGet);

                            }
                        }
                        else
                        {

                            return Json(new
                            {
                                status = "error",
                                msg = "OrgName: Could not find organization."
                            }, JsonRequestBehavior.AllowGet);

                        }
                    }

                    return Json(new
                    {
                        status = "Done",
                        msg = "You Added User successfully."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    msg = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }

            
        }

        // GET: SystemUsers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SystemUsers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SystemUsers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SystemUsers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
