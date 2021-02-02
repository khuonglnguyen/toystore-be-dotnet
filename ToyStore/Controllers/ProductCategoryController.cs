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
    public class ProductCategoryController : Controller
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            this._productCategoryService = productCategoryService;
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ProductCategoryList()
        {
            //Get proudct category list
            var listProductCategory = _productCategoryService.GetProductCategoryList();
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
        public ActionResult EditProductCategory(int id)
        {
            //Get product catetgory
            var productCategory = _productCategoryService.GetByID(id);
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
        public ActionResult EditProductCategory(ProductCategory productCategory, HttpPostedFileBase ImageUpload)
        {
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
            //Set viewbag
            ViewBag.Message = "Cập nhật danh mục thành công!";
            //Update productCategory
            _productCategoryService.UpdateProductCategory(productCategory);
            //Return view
            return View(productCategory);
        }
        [HttpGet]
        public ActionResult DeleteProductCategory(int id)
        {
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
            TempData["message"] = "Xóa danh mục thành công!";
            //Return RedirectToAction
            return RedirectToAction("ProductCategoryList");
        }
    }
}