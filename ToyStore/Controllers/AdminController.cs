using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class AdminController : Controller
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;

        public AdminController(IProductCategoryService productCategoryService)
        {
            this._productCategoryService = productCategoryService;
        }
        #endregion
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ProductCategory()
        {
            var listProductCategory = _productCategoryService.GetProductCategoryList();
            if (listProductCategory != null)
            {
                return View(listProductCategory);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        public ActionResult EditProductCategory(int id)
        {
            var productCategory = _productCategoryService.GetByID(id);
            if (productCategory != null)
            {
                return View(productCategory);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult EditProductCategory(ProductCategory productCategory)
        {
            _productCategoryService.UpdateProductCategory(productCategory);
            return RedirectToAction("ProductCategory");
        }
        [HttpGet]
        public ActionResult DetailsProductCategory(int id)
        {
            var productCategory = _productCategoryService.GetByID(id);
            if (productCategory != null)
            {
                return View(productCategory);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        public ActionResult DeleteProductCategory(int id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var productCategory = _productCategoryService.GetByID(id);
            if (productCategory == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _productCategoryService.DeleteProductCategory(productCategory);
            //ViewBag.Notification = "Xóa danh mục thành công!";
            return RedirectToAction("ProductCategory");
        }
    }
}