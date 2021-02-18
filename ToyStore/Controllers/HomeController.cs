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
        private IProductService _productService;

        public HomeController(IProductCategoryService productCategoryService, IProductService productService)
        {
            this._productCategoryService = productCategoryService;
            _productService = productService;
        }
        #endregion
        public ActionResult Index()
        {
            var ProductCategory1 = _productCategoryService.GetProductCategoryList().SingleOrDefault(x=>x.Name== "Đồ chơi lắp ráp");
            //Get list product 1
            var listProdudct1 = _productService.GetProductList()
                .Where(x => x.HomeFlag == true && x.CategoryID== ProductCategory1.ID)
                .OrderByDescending(x => x.LastUpdatedDate)
                .Take(3);
            ViewBag.ListProduct1 = listProdudct1;

            var ProductCategory2 = _productCategoryService.GetProductCategoryList().SingleOrDefault(x => x.Name == "Đồ chơi Robot");
            //Get list product 2
            var listProdudct2 = _productService.GetProductList()
                .Where(x => x.HomeFlag == true && x.CategoryID == ProductCategory2.ID)
                .OrderByDescending(x => x.LastUpdatedDate)
                .Take(3);
            ViewBag.ListProduct2 = listProdudct2;

            var ProductCategory3 = _productCategoryService.GetProductCategoryList().SingleOrDefault(x => x.Name == "Đồ chơi trí tuệ");
            //Get list product 3
            var listProdudct3 = _productService.GetProductList()
                .Where(x => x.HomeFlag == true && x.CategoryID == ProductCategory3.ID)
                .OrderByDescending(x => x.LastUpdatedDate)
                .Take(3);
            ViewBag.ListProduct3 = listProdudct3;

            return View();
        }
    }
}