using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    public class LookupController : Controller
    {
        private readonly IProductTypeFacade _productTypeFacade;
        private readonly IProductSubtypeFacade _productSubtypeFacade;

        #region CONSTRUCTORS

        public LookupController(IProductTypeFacade productTypeFacade, IProductSubtypeFacade productSubtypeFacade)
        {
            _productTypeFacade = productTypeFacade ?? throw new ArgumentNullException(nameof(productTypeFacade));
            _productSubtypeFacade = productSubtypeFacade ?? throw new ArgumentNullException(nameof(productSubtypeFacade));
        }
        #endregion

        [HttpGet]
        public ActionResult GetAllTypes()
        {
            var response = new AppResponseModel<ProductType>();

            try
            {
                var list = _productTypeFacade.GetAll();

                if (list.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.DataCollection = list;
                    response.Message = Messages.Found;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = true;
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
        public ActionResult GetSubTypes(int id)
        {
            var response = new AppResponseModel<ProductSubType>();

            try
            {
                var list = _productSubtypeFacade.GetByParentId(id);

                if (list.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.DataCollection = list;
                    response.Message = Messages.Found;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = Messages.NoRecord;
                    response.DataCollection = new List<ProductSubType>();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                response.IsSuccess = false;
                response.Message = Messages.ServerError;
                response.DataCollection = new List<ProductSubType>();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}