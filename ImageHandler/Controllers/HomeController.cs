using ImageHandler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageHandler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase uploadfile)
        {
            UploadResponse response = new UploadResponse();
            if (uploadfile != null && uploadfile.ContentLength > 0)
            {
                ImageHandlerDBEntities db = new ImageHandlerDBEntities();
                tbl_Image img = new tbl_Image();
                img.CreateDate = DateTime.Now;
                img.ImageId = Guid.NewGuid();
                img.ImageName = uploadfile.FileName;
                img.ImageType = uploadfile.ContentType;
                using (BinaryReader br = new BinaryReader(uploadfile.InputStream))
                {
                    img.ImageData = br.ReadBytes((int)uploadfile.InputStream.Length);
                }
                db.tbl_Image.Add(img);
                db.SaveChanges();
                response.CreateDate = img.CreateDate;
                response.ImageId = img.ImageId;
                response.ImageType = img.ImageType;
                response.ImageName = img.ImageName;
            }
            return View(response);
        }

        public ActionResult List(int take = 10, int skip = 0)
        {
            var db = new ImageHandlerDBEntities();
            var response = (from img in db.tbl_Image
                            select new UploadResponse
                            {
                                ImageId = img.ImageId,
                                ImageName = img.ImageName,
                                ImageType = img.ImageType,
                                CreateDate = img.CreateDate
                            })
                            .OrderBy(c => c.CreateDate)
                            .Take(take)
                            .Skip(skip)
                            .ToList();
            return View(response);
        }
    }
}