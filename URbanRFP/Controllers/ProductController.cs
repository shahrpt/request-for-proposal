using ICMA.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrbanRFP.Domain.ViewModel;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;

namespace ICMA.Controllers
{
    [RoleAuthorize("Vendor")]
    public class ProductController : BaseController
    {
        private readonly IOrganizationFacade _organizationFacade;
        private readonly IProductTypeFacade _productTypeFacade;
        private readonly IProductSubtypeFacade _productSubtypeFacade;
        private readonly IProductFacade _productFacade;

        #region CONSTRUCTORS

        public ProductController(IProductTypeFacade productTypeFacade, IProductSubtypeFacade productSubtypeFacade, IProductFacade productFacade, IOrganizationFacade organizationFacade)
        {
            _organizationFacade = organizationFacade ?? throw new ArgumentNullException(nameof(organizationFacade));
            _productTypeFacade = productTypeFacade ?? throw new ArgumentNullException(nameof(productTypeFacade));
            _productSubtypeFacade = productSubtypeFacade ?? throw new ArgumentNullException(nameof(productSubtypeFacade));
            _productFacade = productFacade ?? throw new ArgumentNullException(nameof(productFacade));
        }
        #endregion

        public ActionResult Index()
        {
            var result = _productFacade.GetWhere<ProductOrganizationViewModel>($"o.org_key='{CurrentUser.User.org_key}'");
            return View(result);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            var model = new RfpProduct();
            ViewData["Types"] = _productTypeFacade.GetAll();
            ViewData["Orgs"] = _organizationFacade.GetAll();
            return View(model);
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(RfpProduct model)
        {
            ViewData["Types"] = _productTypeFacade.GetAll();
            //ViewData["Orgs"] = _organizationFacade.GetAll();

            var resp_contact = model.RACIContactList.FirstOrDefault(p => p.lk5_key.ToString() == "5592b25d-d98a-4914-9c8e-ac28d6c11116");

            if (resp_contact == null)
            {
                ModelState.AddModelError("", "Please add team member with 'Responsible' role.");
            }

            try
            {
                if(ModelState.IsValid)
                {
                    model.prd_add_date = DateTime.Now;
                    model.prd_org_key = CurrentUser.User.org_key;
                    var response = _productFacade.Add(model, CurrentUser.User.ct_key.ToString());

                    if (response.Success)
                    {
                        // Save added contacts to RACI.
                        foreach (var contact in model.RACIContactList)
                        {
                            _contactFacade.Add_RACI_Contact(contact.lk5_key.ToString(), contact.ct_key.ToString(), null, model.prd_key.ToString(), CurrentUser.User.ct_key.ToString());
                        }
                    }
                }
                else
                {
                    return View(model);
                }

                return RedirectToAction("", "Product");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(string prd_key)
        {
            var model = _productFacade.Find(prd_key);

            if (model.Success)
            {
                model.Result.prd_org_key = CurrentUser.User.org_key;
                model.Result.RACIContactList = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, prd_key = model.Result.prd_key.ToString() });
            }

            ViewData["Types"] = _productTypeFacade.GetAll();
            //ViewData["Orgs"] = _organizationFacade.GetAll();
            return View(model.Result);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(RfpProduct model)
        {
            ViewData["Types"] = _productTypeFacade.GetAll();
            //ViewData["Orgs"] = _organizationFacade.GetAll();

            if (model.RACIContactList == null) model.RACIContactList = new List<ContactRACI>();
            List<ContactRACI> existing_raci_contacts = _contactFacade.GetRACIContacts(new RACIFilter { Org_key = CurrentUser.User.org_key, prd_key = model.prd_key.ToString() });

            var resp_contact = model.RACIContactList.FirstOrDefault(p => p.lk5_key.ToString() == "5592b25d-d98a-4914-9c8e-ac28d6c11116");

            if (resp_contact == null)
            {
                ModelState.AddModelError("", "Please add team member with 'Responsible' role.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    model.prd_change_date = DateTime.Now;
                    model.prd_org_key = CurrentUser.User.org_key;
                    var response = _productFacade.Update(model, string.Empty);

                    if (response.Success)
                    {
                        // Delete removed contacts from RACI.
                        if (existing_raci_contacts != null && existing_raci_contacts.Count > 0)
                        {
                            foreach (var item in existing_raci_contacts)
                            {
                                _contactFacade.Delete_RACI_Contact(item.ct_key.ToString(), null, model.prd_key.ToString());
                            }
                        }

                        // Save added contacts to RACI.
                        foreach (var contact in model.RACIContactList)
                        {
                            _contactFacade.Add_RACI_Contact(contact.lk5_key.ToString(), contact.ct_key.ToString(), null, model.prd_key.ToString(), CurrentUser.User.ct_key.ToString());
                        }
                    }
                }
                else
                {
                    return View(model);
                }

                return RedirectToAction("", "Product");
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
                var result = _productFacade.Remove(id, CurrentUser.User.ct_key.ToString());

                if (result)
                {
                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = "Product deleted successfully!";
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
