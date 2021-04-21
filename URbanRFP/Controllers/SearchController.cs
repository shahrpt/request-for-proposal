using ICMA.Controllers;
using ICMA.Filters;
using ICMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;

namespace UrbanRFP.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchFacade _searchFacade;
        private readonly IRfpRequestFacade _rfpFacade;
        private readonly IMiscFacade _miscFacade;

        #region CONSTRUCTORS


        public SearchController(ISearchFacade searchFacade, IRfpRequestFacade rfpFacade, IMiscFacade miscFacade)
        {
            _searchFacade = searchFacade ?? throw new ArgumentNullException(nameof(searchFacade));
            _rfpFacade = rfpFacade ?? throw new ArgumentNullException(nameof(rfpFacade));
            _miscFacade = miscFacade ?? throw new ArgumentNullException(nameof(miscFacade));
        }
        #endregion

        [AllowAnonymous]
        public ActionResult Result(string searchText = null, string type="prod")
        {
            ViewData["SearchText"] = searchText;

            if(type == "rfp")
                return View("Result-RFP");
            else
                return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchRFPsVendors(string search)
        {
            var response = new AppResponseModel<ProductOrganizationViewModel>();

            SearchFilter filter = Session["SearchProductFilter"] == null ? new SearchFilter() : (SearchFilter)Session["SearchProductFilter"];

            if (string.IsNullOrWhiteSpace(filter.Text))
            {
                filter.Text = string.Empty;
            }

            if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.User.org_key != null)
            {
                filter.OrgKey = SessionHelper.CurrentUser.User.org_key.ToString();
            }

            try
            {
                var RfpProduct = _searchFacade.FindRfpProducts(filter);

                if (RfpProduct.Count() > 0)
                {
                    if(SessionHelper.CurrentUser == null)
                    {
                        if(RfpProduct.Count > 3)
                        {
                            RfpProduct.RemoveRange(3, RfpProduct.Count - 3);
                        }

                        RfpProduct.ForEach(p => p.actions = string.Empty);
                        RfpProduct.ForEach(p => p.prd_key = default(Guid));
                    }
                    else if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.User.per_type == "Vendor")
                    {
                        RfpProduct.ForEach(p => p.actions = $"<div style='float:right'><a href='/rfq?prdkey={p.prd_key}' class='btn btn-default btn-auto'>RFQ</a><button class='btn btn-default btn-auto'>Invite to a RFP</button></div>");
                    }
                    else if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.User.per_type == "RFP")
                    {
                        RfpProduct.ForEach(p => p.actions = string.Empty);
                    }

                    response.IsSuccess = true;
                    response.DataCollection = RfpProduct;
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
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchRFP(string search)
        {
            var response = new AppResponseModel<RfpOrgViewModel>();

            SearchFilter filter = Session["SearchRfpFilter"] == null ? new SearchFilter() : (SearchFilter)Session["SearchRfpFilter"];

            if (string.IsNullOrWhiteSpace(filter.Text))
            {
                filter.Text = string.Empty;
            }

            if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.User.org_key != null)
            {
                filter.OrgKey = SessionHelper.CurrentUser.User.org_key.ToString();
            }

            try
            {
                var result = _rfpFacade.SearchRfp(filter);

                if (result.Count() > 0)
                {
                    if (SessionHelper.CurrentUser == null)
                    {
                        if (result.Count > 3)
                        {
                            result.RemoveRange(3, result.Count - 3);
                        }

                        result.ForEach(p => p.actions = string.Empty);
                        result.ForEach(p => p.rfp_key = default(Guid));
                    }
                    else if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.User.per_type == "RFP")
                    {
                        result.ForEach(p => p.actions = $"<div style='float:right'><a class='btn btn-default btn-auto' disabled>Send a proposal</a></div>");
                    }
                    else if (SessionHelper.CurrentUser != null && SessionHelper.CurrentUser.User.per_type == "Vendor")
                    {
                        result.ForEach(p => p.actions = $"<div style='float:right'><a href='/rfp/create?rfp_key={p.rfp_key}' class='btn btn-default btn-auto'>Send a proposal</a></div>");
                    }

                    result.ForEach(p => p.rfp_summary = string.IsNullOrWhiteSpace(p.rfp_summary) ? "" : Regex.Replace(p.rfp_summary, @"<[^>]*>", String.Empty));

                    response.IsSuccess = true;
                    response.DataCollection = result;
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
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [SessionExpireFilter]
        [RoleAuthorize("Vendor,RFP")]
        public ActionResult IndividualResult(string prdkey = null)
        {
            ViewData["prdkey"] = prdkey;

            var response = new AppResponseModel<ProductOrganizationViewModel>();

            if (string.IsNullOrEmpty(prdkey))
            {
                response.IsSuccess = false;
                response.Message = Messages.NoRecord;
                return View(response);
            }

            try
            {
                var RfpProduct = _searchFacade.FindRfpProduct(prdkey);

                if (RfpProduct != null)
                {
                    if (SessionHelper.CurrentUser != null && (SessionHelper.CurrentUser.User.org_key.ToString() != RfpProduct.org_key.ToString()))
                    {
                        _miscFacade.ViewPage(new CoPageView
                        {
                            pv_key = Guid.NewGuid(),
                            pv_rfp_key = null,
                            pv_prd_key = RfpProduct.prd_key,
                            pv_source_ct_key = SessionHelper.CurrentUser.User.ct_key,
                            pv_add_user = SessionHelper.CurrentUser.User.ct_key.ToString(),
                            pv_view_datetime = DateTime.Now
                        });
                    }

                    response.IsSuccess = true;
                    response.Data = RfpProduct;
                    response.Message = Messages.Found;
                    return View(response);
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = Messages.NoRecord;
                    return View(response);
                }

            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.Message = Messages.ServerError;
                return View(response);
            }
        }

        public ActionResult RFQProgress()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public void SetProductFilter(SearchFilter filter)
        {
            Session["SearchProductFilter"] = filter;
        }

        [AllowAnonymous]
        [HttpPost]
        public void SetRfpFilter(SearchFilter filter)
        {
            Session["SearchRfpFilter"] = filter;
        }
    }
}