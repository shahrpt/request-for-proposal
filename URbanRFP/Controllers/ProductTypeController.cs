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
    public class ProductTypeController : BaseController
    {
        private readonly IProductTypeFacade _productTypeFacade;
        private readonly IProductSubtypeFacade _productSubtypeFacade;

        #region CONSTRUCTORS

        public ProductTypeController(IProductTypeFacade productTypeFacade, IProductSubtypeFacade productSubtypeFacade)
        {
            _productTypeFacade = productTypeFacade ?? throw new ArgumentNullException(nameof(productTypeFacade));
            _productSubtypeFacade = productSubtypeFacade ?? throw new ArgumentNullException(nameof(productSubtypeFacade));
        }
        #endregion

        // GET: ProductType
        public ActionResult Index()
        {
            return View();
        }

        #region Types

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

        public ActionResult AddType(string name)
        {
            var response = new AppResponseModel<ProductType>();

            try
            {
                var result = _productTypeFacade.Add(new ProductType { Name = name }, string.Empty);

                if (result.Success)
                {
                    response.IsSuccess = true;
                    response.Data = result.Result;
                    response.Message = Messages.Created;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.Message;
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

        public ActionResult UpdateType(int id, string name)
        {
            var response = new AppResponseModel<ProductType>();

            try
            {
                var result = _productTypeFacade.Update(new ProductType { Name = name, Id = id }, string.Empty);

                if (result.Success)
                {
                    response.IsSuccess = true;
                    response.Data = result.Result;
                    response.Message = Messages.Created;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.Message;
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

        public ActionResult DeleteType(string id)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var result = _productTypeFacade.Remove(id, string.Empty);

                if (result)
                {
                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = Messages.Removed;
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

        #endregion

        #region Subtype

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

        

        public ActionResult AddSubtype(int parentid, string name)
        {
            var response = new AppResponseModel<ProductSubType>();

            try
            {
                var result = _productSubtypeFacade.Add(new ProductSubType { Name = name, ParentID = parentid}, string.Empty);

                if (result.Success)
                {
                    response.IsSuccess = true;
                    response.Data = result.Result;
                    response.Message = Messages.Created;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.Message;
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

        public ActionResult UpdateSubtype(int id, string name, int parentid)
        {
            var response = new AppResponseModel<ProductSubType>();

            try
            {
                var result = _productSubtypeFacade.Update(new ProductSubType { Name = name, Id = id, ParentID= parentid }, string.Empty);

                if (result.Success)
                {
                    response.IsSuccess = true;
                    response.Data = result.Result;
                    response.Message = Messages.Created;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = result.Message;
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

        public ActionResult DeleteSubtype(string id)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var result = _productSubtypeFacade.Remove(id, string.Empty);

                if (result)
                {
                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = Messages.Removed;
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
        #endregion
    }
}
