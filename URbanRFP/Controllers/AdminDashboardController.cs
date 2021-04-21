using ICMA.Filters;
using ICMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Models;


namespace ICMA.Controllers
{
    public class AdminDashboardController : BaseController
    {
        private readonly IRfpRequestFacade _rfpRequestFacade;
        private readonly IRfpAttachmentFacade _rfpAttachmentFacade;

        #region CONSTRUCTORS

        public AdminDashboardController(IRfpRequestFacade rfpRequestFacade, IRfpAttachmentFacade rfpAttachmentFacade)
        {
            _rfpRequestFacade = rfpRequestFacade ?? throw new ArgumentNullException(nameof(rfpRequestFacade));
            _rfpAttachmentFacade = rfpAttachmentFacade ?? throw new ArgumentNullException(nameof(rfpAttachmentFacade));
        }
        #endregion


        [RoleAuthorize("Vendor, Admin")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminDashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminDashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminDashboard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminDashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminDashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminDashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminDashboard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
