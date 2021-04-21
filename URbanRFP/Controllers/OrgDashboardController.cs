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
    [RoleAuthorize("RFP,Vendor")]
    public class OrgDashboardController : BaseController
    {

        #region CONSTRUCTORS

        private UrbanRFPEntities db = new UrbanRFPEntities();

        public OrgDashboardController()
        {
        }

        #endregion
        // GET: OrgDashboard
        public ActionResult Index()
        {
            return View();
        }


        #region Organization Data
        public async Task<PartialViewResult> OrgData()
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            byte flag = 1;

            var org = await db.contact_x_organization.Where(w => w.cxo_delete_flag!=flag 
            && w.cxo_ct_key.ToString() == currUser).FirstOrDefaultAsync();

            return PartialView(org);
        }


        public async Task<PartialViewResult> EditOrgData()
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            byte flag = 1;

            var org = await db.contact_x_organization.Where(w => w.cxo_delete_flag != flag
            && w.cxo_ct_key.ToString() == currUser).FirstOrDefaultAsync();

            return PartialView(org.co_organization);
        }
        
        [HttpPost]
        public async Task<JsonResult> SaveOrg(co_organization data, string upID)
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            Guid recID = new Guid();
            try
            {
                var gID = Guid.Parse(upID);
                var updata = await db.co_organization.Where(w => w.org_key == gID).FirstOrDefaultAsync();
                
                updata.org_change_date = DateTime.Now;
                updata.org_change_user = currUser;

                updata.org_ann_revenue = data.org_ann_revenue;
                updata.org_date_created = data.org_date_created;
                updata.org_description = data.org_description;
                updata.org_entity_type = data.org_entity_type;
                updata.org_federal_tax_id = data.org_federal_tax_id;
                updata.org_legal_name = data.org_legal_name;
                updata.org_specialities = data.org_specialities;
                updata.org_timezone = data.org_timezone;
                updata.org_vendor_number_of_employees = data.org_vendor_number_of_employees;
                updata.org_website = data.org_website;

                db.Entry(updata).State = EntityState.Modified;
                await db.SaveChangesAsync();

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


        #endregion

        #region Addresses Data


        public async Task<PartialViewResult> addAddress(string id)
        {
            ViewBag.keyOrg = id;
            var orgKey = Guid.Parse(id);
            return PartialView();
        }


        [HttpPost]
        public async Task<JsonResult> SaveAddress(co_address data, string keyOrg)
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            try
            {
                data.adr_key = Guid.NewGuid();
                data.adr_add_date = DateTime.Now;
                data.adr_add_user = currUser;

                db.co_address.Add(data);
                await db.SaveChangesAsync();

                var addressOrg = new address_x_organization()
                {
                    oxa_key = Guid.NewGuid(),
                    oxa_adr_key = data.adr_key,
                    oxa_org_key = Guid.Parse(keyOrg),
                    oxa_add_user = currUser,
                    oxa_add_date = DateTime.Now
                };
                db.address_x_organization.Add(addressOrg);
                await db.SaveChangesAsync();
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
        [HttpPost]
        public async Task<JsonResult> removeAddress(string id)
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            try{
               
                var gID = Guid.Parse(id);
                var updata = await db.address_x_organization.Where(w => w.oxa_key == gID).FirstOrDefaultAsync();
               
                updata.oxa_delete_flag = (byte)1;
                updata.oxa_change_user = currUser;
                updata.oxa_change_date = DateTime.Now;
                
                db.Entry(updata).State = EntityState.Modified;
                await db.SaveChangesAsync();

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
        
        //SaveAddress
        #endregion

        #region User Data 
        public async Task<PartialViewResult> UserData()
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            var users = await db.co_contact.Where(w => w.ct_key.ToString()== currUser).FirstOrDefaultAsync();
            return PartialView(users);
        }


        // GET: SystemUsers/Create
        public async Task<PartialViewResult> EditUserDate(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                string currUser = CurrentUser.User.ct_key.ToString();
                ViewBag.CID = id;
                var user = await db.co_contact.Where(w => w.ct_key.ToString() == currUser).FirstOrDefaultAsync();
                if (user == null)
                    throw new Exception("User Not Fount");

                return PartialView(user);
            }
            else
            {
                throw new Exception("User Not Fount");
            }

        }


        [HttpPost]
        public async Task<JsonResult> SaveUser(co_contact data)
        {
            string currUser = CurrentUser.User.ct_key.ToString();
            Guid recID = new Guid();
            try
            {
                var gID = Guid.Parse(currUser);
                var updata = await db.co_contact.Where(w => w.ct_key == gID).FirstOrDefaultAsync();

                var Email = data.ct_email + updata.ct_email.Substring(updata.ct_email.IndexOf("@"));

                var existEmail = await db.co_contact.Where(w => w.ct_key != gID
                && w.ct_email.ToLower().Trim() == Email.ToLower().Trim()).FirstOrDefaultAsync();
                if (existEmail != null)
                {
                    return Json(new
                    {
                        status = "error",
                        msg = "This email is already exist"
                    }, JsonRequestBehavior.AllowGet);
                }

                updata.ct_change_date = DateTime.Now;
                updata.ct_change_user = currUser;
                updata.ct_email = Email;
                updata.ct_fax = data.ct_fax;
                updata.ct_first_name = data.ct_first_name;
                updata.ct_last_name = data.ct_last_name;
                updata.ct_phone = data.ct_phone;

                db.Entry(updata).State = EntityState.Modified;
                await db.SaveChangesAsync();
                recID = updata.ct_key;

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

        public async Task<PartialViewResult> ChangePassword()
        {

            string currUser = CurrentUser.User.ct_key.ToString();
            var user = await db.co_contact.Where(w => w.ct_key.ToString() == currUser).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("User Not Fount");

            return PartialView();

        }
        [HttpPost]
        public async Task<JsonResult> ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            Guid recID = new Guid();
            try
            {
                if (NewPassword != ConfirmPassword)
                    throw new Exception("Password Must be Matching.");

                string currUser = CurrentUser.User.ct_key.ToString();
                var gID = Guid.Parse(currUser);
                var updata = await db.co_contact.Where(w => w.ct_key == gID).FirstOrDefaultAsync();
                if (OldPassword.GetEncryptedString() != updata.ct_password)
                    throw new Exception("Invalid Old Passowrd.");
                updata.ct_change_date = DateTime.Now;
                updata.ct_change_user = currUser;

                updata.ct_password = NewPassword.GetEncryptedString();

                db.Entry(updata).State = EntityState.Modified;
                await db.SaveChangesAsync();
                recID = updata.ct_key;

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
        #endregion


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
