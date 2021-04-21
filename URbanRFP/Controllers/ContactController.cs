using ICMA.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Helpers;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactFacade _contactFacade;
        private readonly IOrganizationFacade _orgFacade;

        #region CONSTRUCTORS

        public ContactController(IContactFacade contactFacade, IOrganizationFacade orgFacade)
        {
            _contactFacade = contactFacade ?? throw new ArgumentNullException(nameof(contactFacade));
            _orgFacade = orgFacade ?? throw new ArgumentNullException(nameof(orgFacade));
        }

        #endregion

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRACI(string[] contact_keys)
        {
            ViewData["RACI"] = _contactFacade.GetRACI();
            var orgResp = _orgFacade.Find(CurrentUser.User.org_key.ToString());
            var model = new RACIViewModel();
            model.Organization = orgResp.Success ? orgResp.Result : new CoOrganization();
            model.Contacts = GetFilterContacts(contact_keys);

            return PartialView("RACI", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RACI(RACIViewModel model, string[] contact_keys)
        {
            ViewData["RACI"] = _contactFacade.GetRACI();
            var orgResp = _orgFacade.Find(CurrentUser.User.org_key.ToString());
            model.Organization = orgResp.Success ? orgResp.Result : new CoOrganization();
            
            if (ModelState.IsValid)
            {
                var contact = new CoContact()
                {
                    ct_email = model.NewContact.Email,
                    ct_password = model.NewContact.Password.GetEncryptedString(),
                    ct_first_name = model.NewContact.FirstName,
                    ct_last_name = model.NewContact.LastName,
                    ct_phone = model.NewContact.Phone,
                    ct_fax = model.NewContact.Fax,
                    ct_key = Guid.NewGuid(),
                    ct_add_user = CurrentUser.User.ct_key.ToString(),
                    ct_activation_code = Guid.NewGuid()
                };

                try
                {
                    bool isEmail = Regex.IsMatch(model.NewContact.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                    if (!isEmail)
                    {
                        ModelState.AddModelError("NewContact.Email", "Enter valid email address.");
                        model.Contacts = GetFilterContacts(contact_keys);
                        return PartialView(model);
                    }

                    var existingUser = _contactFacade.GetByUsername(model.NewContact.Email.Trim());

                    if (existingUser.Success)
                    {
                        ModelState.AddModelError("NewContact.Email", "This email is already registered");
                        model.Contacts = GetFilterContacts(contact_keys);
                        return PartialView(model);
                    }
                    else
                    {
                        _contactFacade.Add(contact, CurrentUser.User.ct_key.ToString());
                        _contactFacade.Add_Contact_Org(CurrentUser.User.org_key.ToString(), contact.ct_key.ToString(), CurrentUser.User.ct_key.ToString());
                        TempData["Message"] = "New contact has been created successfully.";
                        //EmailManager.SendWelcome(contact.ct_email, string.Format("{0} {1}", contact.ct_first_name, contact.ct_last_name), contact.ct_activation_code.ToString());
                        model.NewContact = new RegisterViewModel();
                        model.Contacts = GetFilterContacts(contact_keys);
                        return PartialView(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    model.Contacts = GetFilterContacts(contact_keys);
                    return PartialView(model);
                }
            }

            // If we got this far, something failed, redisplay form
            model.Contacts = GetFilterContacts(contact_keys);
            return PartialView(model);
        }

        private List<UserProfile> GetFilterContacts(string[] excluded_contacts)
        {
            List<UserProfile>  all_contacts = _contactFacade.GetOrgContacts(CurrentUser.User.org_key);

            if (excluded_contacts != null && excluded_contacts.Length > 0 && all_contacts != null)
            {
                foreach (var ct_key in excluded_contacts)
                {
                    all_contacts.RemoveAll(p => p.ct_key.ToString() == ct_key);
                }
            }

            return all_contacts;
        }
    }
}