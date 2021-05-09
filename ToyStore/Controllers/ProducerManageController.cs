using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class ProducerManageController : Controller
    {
        #region Initialize
        private IProducerService _producerService;

        public ProducerManageController(IProducerService producerService)
        {
            _producerService = producerService;
        }
        #endregion
        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            int pageSize = 5;
            //Get proudcer
            var producers = _producerService.GetProducerList();
            PagedList<Producer> listProducer = new PagedList<Producer>(producers, page, pageSize);
            //Check null
            if (listProducer != null)
            {
                ViewBag.Page = page;
                //Return view
                return View(listProducer);
            }
            else
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        //[HttpPost]
        //public JsonResult ListName(string Prefix)
        //{
        //    List<string> names = _productCategoryService.GetProductCategoryListName(Prefix);
        //    return Json(names, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public ActionResult List(string keyword)
        //{
        //    if (Session["Emloyee"] == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    //Get data for DropdownList
        //    ViewBag.ParentIDEdit = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name");
        //    int pageSize = 5;
        //    if (keyword == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    //Get proudct category list with keyword
        //    var ProductCategories = _productCategoryService.GetProductCategoryList(keyword);
        //    PagedList<ProductCategory> listProductCategory = new PagedList<ProductCategory>(ProductCategories, 1, pageSize);
        //    //Check null
        //    if (listProductCategory != null)
        //    {
        //        //Return view
        //        return View(listProductCategory);
        //    }
        //    else
        //    {
        //        //return 404
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //}
        [HttpPost]
        public ActionResult Create(Producer producer, HttpPostedFileBase ImageUpload, int page)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Declare a errorCount
            int errorCount = 0;
            //Check content image
            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                //Check format iamge
                if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                {
                    //Set viewbag
                    ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                    //increase by 1 unit errorCount
                    errorCount++;
                }
                else
                {
                    //Get file name
                    var fileName = Path.GetFileName(ImageUpload.FileName);
                    //Get path
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Check exitst
                    if (!System.IO.File.Exists(path))
                    {
                        //Add image into folder
                        ImageUpload.SaveAs(path);
                    }
                }
                if (errorCount > 0)
                {
                    ViewBag.Messag = "Failed";
                    return View(producer);
                }
                //Set new value image for productCategory
                producer.Logo = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["create"] = "Success";
            //Create producer
            _producerService.AddProducer(producer);
            //Return view
            return RedirectToAction("Index", new { page = 1 });
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get producer
            var producer = _producerService.GetByID(id);
            //Check null
            if (producer != null)
            {
                //Return view
                return Json(new
                {
                    ID = producer.ID,
                    Name = producer.Name,
                    Imfomation = producer.Imfomation,
                    Logo = producer.Logo,
                    IsActive = producer.IsActive,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Edit(Producer producer, HttpPostedFileBase ImageUpload, int page)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Declare a errorCount
            int errorCount = 0;
            //Check content image
            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                //Check format iamge
                if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                {
                    //Set viewbag
                    ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                    //increase by 1 unit errorCount
                    errorCount++;
                }
                else
                {
                    //Get file name
                    var fileName = Path.GetFileName(ImageUpload.FileName);
                    //Get path
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Check exitst
                    if (!System.IO.File.Exists(path))
                    {
                        //Add image into folder
                        ImageUpload.SaveAs(path);
                    }
                }
                if (errorCount > 0)
                {
                    ViewBag.Messag = "Cập nhật nhà sản xuất thât bại!";
                    return View(producer);
                }
                //Set new value image for producer
                producer.Logo = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update producer
            _producerService.UpdateProducer(producer);
            //Return view
            return RedirectToAction("Index", new { page = page });
        }
        public void Block(int id)
        {
            //Get producer
            var producer = _producerService.GetByID(id);
            //Block producer
            _producerService.Block(producer);
        }
        public void Active(int id)
        {
            //Get producer
            var producer = _producerService.GetByID(id);
            //Block producer
            _producerService.Active(producer);
        }
        //[HttpPost]
        //public ActionResult DeleteMulti(FormCollection formCollection)
        //{
        //    if (Session["Emloyee"] == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    string[] Ids = formCollection["IDDelete"].Split(new char[] { ',' });
        //    _productCategoryService.MultiDeleteProductCategory(Ids);
        //    //Set TempData for checking in view to show swal
        //    TempData["deleteMulti"] = "Success";
        //    return RedirectToAction("Index");
        //}
    }
}