using System;
using System.Web.Mvc;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationFacade _organizationFacade;

        #region CONSTRUCTORS

        public OrganizationController(IOrganizationFacade organizationFacade)
        {
            _organizationFacade = organizationFacade ?? throw new ArgumentNullException(nameof(organizationFacade));
        }
        #endregion

        // GET: Organization
        public ActionResult Index()
        {
            var orgs = _organizationFacade.GetAll();
            return View(orgs);
        }

        public ActionResult Create()
        {
            var model = new CoOrganization();
            return View(model);
        }

        // POST: Organization/Create
        [HttpPost]
        public ActionResult Create(CoOrganization model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.org_add_date = DateTime.Now;
                    _organizationFacade.Add(model, string.Empty);
                }
                else
                {
                    return View(model);
                }

                return RedirectToAction("Index", "Organization");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Organization/Edit/Org_key
        public ActionResult Edit(string org_key)
        {
            var model = _organizationFacade.Find(org_key);
            return View(model.Result);
        }

        // POST: Organization/Edit/Org_key
        [HttpPost]
        public ActionResult Edit(CoOrganization model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.org_change_date = DateTime.Now;
                    _organizationFacade.Update(model, string.Empty);
                }
                else
                {
                    return View(model);
                }

                return RedirectToAction("Index", "Organization");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(string id)
        {
            var response = new AppResponseModel<bool>();

            try
            {
                var result = _organizationFacade.Remove(id, string.Empty);

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
    }
}