using ICMA.Filters;
using ICMA.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Facade;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Interfaces;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    [SessionExpireFilter]
    //[Authorize]
    public class BaseController : Controller
    {
        #region Private Fields

        private readonly Container container;

        /// <summary>
        /// Represents current logged in user.
        /// </summary>
        private CurrentUser _currentUser;

        protected readonly IContactFacade _contactFacade;
        protected readonly IMiscFacade _miscFacade;

        #endregion

        #region Properties

        /// <summary>
        /// Represents current logged-in user.
        /// </summary>
        public virtual CurrentUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                { _currentUser = SessionHelper.CurrentUser; }

                if (_currentUser == null)
                {
                    _currentUser = new CurrentUser(_contactFacade.GetByUsername(User.Identity.Name).Result);
                }

                return _currentUser;
            }
        }

        #endregion

        #region CONSTRUCTORS
        public BaseController()
        {
            container = UrbanRFP.SimpleInjectorConfig.container;
            _contactFacade = container.GetInstance<ContactFacade>();
            _miscFacade = container.GetInstance<MiscFacade>();
        }

        #endregion

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            // now Server has been initialized
            string uploadPath = Server.MapPath("~/content/uploads/");

            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
        }

        [HttpPost]
        public ActionResult AddToFavorite(string key, string type, bool status)
        {
            var response = new AppResponseModel<int>();

            try
            {
                if (type == "rfp" || type == "prd")
                {
                    CoFavorite entity = new CoFavorite
                    {
                        is_favorite = status,
                        fav_org_key = CurrentUser.User.org_key,
                        fav_add_date = DateTime.Now,
                        fav_key = Guid.NewGuid(),
                        fav_add_user = CurrentUser.User.ct_key.ToString()
                    };

                    if (type == "prd")
                        entity.fav_prd_key = Guid.Parse(key);
                    else
                        entity.fav_prd_key = null;

                    if (type == "rfp")
                        entity.fav_rfp_key = Guid.Parse(key);
                    else
                        entity.fav_rfp_key = null;

                    var resp = _miscFacade.MarkFavorite(entity);

                    if (resp.Success)
                    {
                        response.IsSuccess = true;
                        response.Message = Messages.Updated;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }

                response.IsSuccess = true;
                response.Message = Messages.NoUpdate;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}