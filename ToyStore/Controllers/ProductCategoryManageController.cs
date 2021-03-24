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
    public class ProductCategoryManageController : Controller
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;
        private IProductCategoryParentService _productCategoryParentService;

        public ProductCategoryManageController(IProductCategoryService productCategoryService,ProductCategoryParentService productCategoryParentService)
        {
            this._productCategoryService = productCategoryService;
            this._productCategoryParentService = productCategoryParentService;
        }
        #endregion

        public ActionResult Index()
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult List(int page = 1)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            int pageSize = 5;
            //Get proudct category list
            var productCategories = _productCategoryService.GetProductCategoryList();
            PagedList<ProductCategory> listProductCategory = new PagedList<ProductCategory>(productCategories, page, pageSize);
            //Check null
            if (listProductCategory != null)
            {
                //Return view
                return View(listProductCategory);
            }
            else
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public JsonResult ListName(string Prefix)
        {
            var listProductCategoryListName = _productCategoryService.GetProductCategoryListName(Prefix).ToList();
            return Json(listProductCategoryListName, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult List(string keyword)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            int pageSize = 5;
            if (keyword == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get proudct category list with keyword
            var ProductCategories = _productCategoryService.GetProductCategoryList(keyword);
            PagedList<ProductCategory> listProductCategory = new PagedList<ProductCategory>(ProductCategories, 1, pageSize);
            //Check null
            if (listProductCategory != null)
            {
                //Return view
                return View(listProductCategory);
            }
            else
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.ParentID = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name");
            //Return view
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory, HttpPostedFileBase ImageUpload)
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
                    return View(productCategory);
                }
                //Set new value image for productCategory
                productCategory.Image = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["create"] = "Success";
            //Create productCategory
            _productCategoryService.AddProductCategory(productCategory);
            //Return view
            return RedirectToAction("List");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get product catetgory
            var productCategory = _productCategoryService.GetByID(id);
            //Get data for DropdownList
            ViewBag.ParentID = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name", productCategory.ParentID);
            //Check null
            if (productCategory != null)
            {
                //Return view
                return View(productCategory);
            }
            else
            {
                //Return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, HttpPostedFileBase ImageUpload)
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
                    ViewBag.Messag = "Cập nhật danh mục thât bại!";
                    return View(productCategory);
                }
                //Set new value image for productCategory
                productCategory.Image = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update productCategory
            _productCategoryService.UpdateProductCategory(productCategory);
            //Return view
            return RedirectToAction("List");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Check id null
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get productCategory by ID
            var productCategory = _productCategoryService.GetByID(id);
            //Check null
            if (productCategory == null)
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Delete productCategory
            _productCategoryService.DeleteProductCategory(productCategory);
            //Set TempData for checking in view to show swal
            TempData["delete"] = "Success";
            //Return RedirectToAction
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult DeleteMulti(FormCollection formCollection)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            string[] Ids = formCollection["IDDelete"].Split(new char[] { ',' });
            _productCategoryService.MultiDeleteProductCategory(Ids);
            //Set TempData for checking in view to show swal
            TempData["deleteMulti"] = "Success";
            return RedirectToAction("List");
        }
    }
}