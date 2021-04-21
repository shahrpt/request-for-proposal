using ICMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    public class GeneralController : Controller
    {
        private readonly IGeneralFacade _generalFacade;

        #region CONSTRUCTORS

        public GeneralController(IGeneralFacade searchFacade)
        {
            _generalFacade = searchFacade ?? throw new ArgumentNullException(nameof(searchFacade));
        }
        #endregion

        public ActionResult GetMenuItems()
        {
            var response = new AppResponseModel<MenuViewModel>();

            try
            {
                var menus = _generalFacade.GetMenuItems();

                if (menus != null)
                {
                    response.IsSuccess = true;
                   
                    response.Message = Messages.Found;

                    List<MenuViewModel> filterMenus = new List<MenuViewModel>();

                    foreach (var menu in menus)
                    {
                        if (menu.MenuParentID == null)
                        {
                            //if (menus.Where(x => x.MenuParentID == menu.MenuID).Count() > 0)
                            //{
                            //    var subMenus = menus.Where(x => x.MenuParentID == menu.MenuID).ToList();
                            //    foreach (var subMenu in subMenus)
                            //    {
                            //        menu.SubMenu.Add(subMenu);
                            //    }
                            //}
                            filterMenus.Add(menu);
                        }
                    }

                    if (SessionHelper.CurrentUser != null)
                    {
                        var menu = filterMenus.FirstOrDefault(p => p.MenuID == 6);

                        if(menu != null)
                        {
                            menu.MenuLink = SessionHelper.CurrentUser.User.per_type == "RFP" ? "/rfp" : "/product";
                        }
                    }

                    response.DataCollection = filterMenus;

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = Messages.NoRecord;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}