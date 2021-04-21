using ICMA.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Infrastructure.Core;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;
using UrbanRFP.Infrastructure.Helpers;
using System.Text.RegularExpressions;
using UrbanRFP.Helpers;
using System.Web.Script.Serialization;

namespace UrbanRFP.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthFacade authService;
        private readonly IContactFacade contactService;
        private readonly IOrganizationFacade orgService;

        public AccountController(IAuthFacade _authService, IContactFacade _contactService, IOrganizationFacade _orgService)
        {
            this.authService = _authService ?? throw new ArgumentNullException(nameof(_authService));
            this.contactService = _contactService ?? throw new ArgumentNullException(nameof(_contactService));
            this.orgService = _orgService ?? throw new ArgumentNullException(nameof(_orgService));
        }
        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Unauthorized()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(returnUrl)) returnUrl = "/dashboard";

            var response = authService.Authenticate(model.UserName, model.Password);

            switch (response.Status)
            {
                case Status.OK:
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    SessionHelper.CurrentUser = response.Result;
                    bool isPersistent = true;
                    DateTime expiration = DateTime.Now.AddMinutes(120);
                    CreateFormsAuthenticationTicket(response.Result.User.ct_email, response.Result.User.ct_key.ToString(), isPersistent, expiration);
                    return RedirectToLocal(returnUrl);
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contact = new CoContact() {
                    ct_email = model.Email,
                    ct_password = model.Password.GetEncryptedString(),
                    ct_first_name = model.FirstName,
                    ct_last_name = model.LastName,
                    ct_phone = model.Phone,
                    ct_fax = model.Fax,
                    ct_key = Guid.NewGuid(),
                    ct_add_user = "public",
                    ct_activation_code = Guid.NewGuid()
                };

                try
                {
                    bool isEmail = Regex.IsMatch(model.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                    if (!isEmail)
                    {
                        ModelState.AddModelError("Email", "Enter valid email address.");
                        return View(model);
                    }

                    var result = orgService.GetWhere($"org_domain='{model.OrgDomain.Trim()}'");

                    if ((result == null || (result != null && result.Count == 0)) && string.IsNullOrWhiteSpace(model.OrgName))
                    {
                        ViewData["ShowOrg"] = true;
                        ModelState.AddModelError("OrgName", "Enter valid organization name.");
                        return View(model);
                    }

                    var existingUser = contactService.GetByUsername(model.Email.Trim());

                    if (existingUser.Success)
                    {
                        ModelState.AddModelError("Email", "This email is already registered");
                        return View(model);
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
                            var org_resp = orgService.Add(new CoOrganization {
                                org_add_date = DateTime.Now,
                                org_key = Guid.NewGuid(),
                                org_legal_name = model.OrgName.Trim(),
                                org_domain = model.OrgDomain,
                                org_add_user = "public"
                            }, "public");

                            if (org_resp.Success)
                            {
                                org = org_resp.Result;
                                var response = contactService.Add(contact, "public");

                                if (response.Success)
                                {
                                    contactService.Add_Contact_Org(org.org_key.ToString(), contact.ct_key.ToString(), "public");
                                    TempData["Message"] = "You have been registered successfully. Please check your email to activate account.";
                                    EmailManager.SendWelcome(contact.ct_email, string.Format("{0} {1}", contact.ct_first_name, contact.ct_last_name), contact.ct_activation_code.ToString());
                                    return RedirectToAction("Login");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Could not register you. Please contact support.");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("OrgName", "Could not find organization.");
                            }
                        }

                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CheckUserExists(string email)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var existingUser = contactService.GetByUsername(email.Trim());

                if (existingUser.Success && existingUser.Result.ct_email.Length > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "This email is already registered";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.NoRecord;
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CheckOrgExists(string domain)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var result = orgService.GetWhere($"org_domain='{domain.Trim()}'");

                if (result!= null && result.Count > 0)
                {
                    response.IsSuccess = true;
                    response.Message = Messages.Found;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = Messages.NoRecord;
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

        /// <summary>
        /// Create Form Auth ticket
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userID"></param>
        /// <param name="isPersistent"></param>
        /// <param name="expiration"></param>
        protected void CreateFormsAuthenticationTicket(string userName, string userID, bool isPersistent, DateTime expiration)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                userName,                       
                DateTime.Now,                   
                expiration,                     
                isPersistent,                   
                userID                        
             );

            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            cookie.HttpOnly = true;
           
            if (isPersistent)
            {
                cookie.Expires = expiration;
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }

            Response.Cookies.Add(cookie);
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult Activate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Error"] = "Invaid activation code. User couldn't be activated.";
            }
            else
            {
                var response = contactService.Activate(id);

                if (response.Success)
                {
                    TempData["Message"] = "Your account has been activated successfully. Please login to use it.";
                }
                else
                {
                    TempData["Error"] = "Invaid activation code. User couldn't be activated.";
                }
            }

            return RedirectToAction("Login");
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Regex.IsMatch(model.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                    if (!isEmail)
                    {
                        ModelState.AddModelError("", "Enter valid email address.");
                        return View(model);
                    }

                    var contact = contactService.GetByUsername(model.Email);

                    if (!contact.Success)
                    {
                        ViewBag.Error = "User does not exists!";
                        return View(model);
                    }
                    else
                    {
                        var user_data = new { ct_key = contact.Result.ct_key, Date = DateTime.Now };
                        var serializer = new JavaScriptSerializer();
                        var serializedResult = serializer.Serialize(user_data).Base64Encode();

                        EmailManager.SendResetPassword(contact.Result.ct_email, string.Format("{0} {1}", contact.Result.ct_first_name, contact.Result.ct_last_name), serializedResult);
                        TempData["Message"] = "An email has been sent with secure link to reset password. Please reset your password.";
                        return RedirectToAction("Login");
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string id)
        {
            try
            {
                string decodedString = id.Base64Decode();
                var serializer = new JavaScriptSerializer();
                var deserializedData = serializer.Deserialize<dynamic>(decodedString);

                Response<CoContact> contact = contactService.Find(deserializedData["ct_key"]);

                if (!contact.Success)
                {
                    TempData["Error"] = "User couldn't be found!";
                    return RedirectToAction("Login");
                }
                else
                {
                    var model = new ResetPasswordViewModel { Code = contact.Result.ct_key.ToString(), Email = contact.Result.ct_email };
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "User couldn't be found!";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contact = contactService.Find(model.Code);
                    var result = contactService.UpdatePassword(model.Code, model.Password.GetEncryptedString());

                    if (result.Success)
                    {
                        TempData["Message"] = "Your password has been changed successfully!";
                        EmailManager.SendResetPasswordConfirmation(contact.Result.ct_email, string.Format("{0} {1}", contact.Result.ct_first_name, contact.Result.ct_last_name));
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["Error"] = "Couldn't change password!";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Couldn't change password!";
                return RedirectToAction("Login");
            }
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }
        }
        #endregion
    }
}