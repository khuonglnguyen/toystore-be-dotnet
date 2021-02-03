using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Service;
using ToyStore.Data;
using ToyStore.Data.Repository;
using ToyStore.Models;

namespace ToyStore.Controllers
{
    public class HomeController : Controller
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;

        public HomeController(IProductCategoryService productCategoryService)
        {
            this._productCategoryService = productCategoryService;
        }
        #endregion
        public ActionResult Index()
        {
            //ViewBag.ProducerID = new SelectList(_product.Pro.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            var listProduct= _productCategoryService.GetProductCategoryList();
            if (listProduct != null)
            {
                return View(listProduct);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}