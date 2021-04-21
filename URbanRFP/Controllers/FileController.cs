using ICMA.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrbanRFP.Facade.Interfaces;
using UrbanRFP.Helpers;
using UrbanRFP.Infrastructure.Entity;
using UrbanRFP.Infrastructure.Helpers;

namespace ICMA.Controllers
{
    public class FileController : Controller
    {
        private readonly IRfpAttachmentFacade _rfpAttachmentFacade;

        #region CONSTRUCTORS

        public FileController(IRfpAttachmentFacade rfpAttachmentFacade)
        {
            _rfpAttachmentFacade = rfpAttachmentFacade ?? throw new ArgumentNullException(nameof(rfpAttachmentFacade));
        }
        #endregion

        [HttpPost]
        public ActionResult Index()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string baseFolderPath = Server.MapPath(ConfigHelper.GetSettingAsString("UploadsFolderPath"));
                    string folderPath = DateTime.Today.ToString("yyyyMM");
                    string fileName = Guid.NewGuid() + Path.GetFileNameWithoutExtension(file.FileName).RemoveSpecialChars() + Path.GetExtension(file.FileName);
                    string fillFullName = Path.Combine(baseFolderPath, folderPath, fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(fillFullName));

                    if (file.IsImage())
                        Image.FromStream(file.InputStream).SaveImage(Path.Combine(baseFolderPath, folderPath), fileName);
                    else
                        file.SaveAs(fillFullName);

                    RfpAttachment upload = new RfpAttachment
                    {
                        rfa_key = Guid.NewGuid(),
                        rfa_rfp_key = null,
                        rfa_rfr_key = null,
                        rfa_cxr_key = null,
                        rfa_prd_key = null,
                        rfa_org_key = null,
                        rfa_file_name = fileName,
                        rfa_file_path = folderPath,
                        rfa_mime = file.ContentType,
                        rfa_add_date = DateTime.Now,
                        rfa_add_user = string.Empty,
                    };

                    var result = _rfpAttachmentFacade.Add(upload, "");

                    return Json(new { rfa_key = upload.rfa_key, message = "success", Url = Path.Combine(Utility.GetSetting("UploadsURL"), folderPath, fileName) }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { message = "error" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FileUploader(string name = "rfa_key", int maxFiles = 1, string rfa_key = "", string ViewMode = "", string PicId = "")
        {
            ViewBag.Name = name;
            ViewBag.PicId = PicId;
            ViewBag.MaxFiles = maxFiles;
            ViewBag.rfa_key = rfa_key;
            ViewBag.ViewMode = ViewMode;
            return PartialView();
        }

        public ActionResult GetFileDetail(string id)
        {
            var result = string.IsNullOrWhiteSpace(id)? null : _rfpAttachmentFacade.Find(id);
            return Json(result==null? null :result.Result, JsonRequestBehavior.AllowGet);
        }
    }
}