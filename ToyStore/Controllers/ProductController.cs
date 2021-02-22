using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class ProductController : Controller
    {
        #region Initialize
        private IProductService _productService;
        private IProducerService _producerService;
        private ISupplierService _supplierService;
        private IProductCategoryService _productCategoryService;
        private IAgeService _ageService;

        public ProductController(IProductService productService, IProducerService producerService, ISupplierService supplierService, IProductCategoryService productCategoryService, IAgeService ageService)
        {
            this._productService = productService;
            this._producerService = producerService;
            this._supplierService = supplierService;
            this._productCategoryService = productCategoryService;
            this._ageService = ageService;
        }
        #endregion
        // GET: Product
        public ActionResult Details(int ID)
        {
            var product = _productService.GetByID(ID);
            var producer = _producerService.GetByID(product.ProducerID);
            var supplier = _supplierService.GetByID(product.SupplierID);
            var listProduct = _productService.GetProductList().OrderByDescending(x=>x.ViewCount).Take(5);
            ViewBag.ProducerName = producer.Name;
            ViewBag.SupplierName = supplier.Name; 
            ViewBag.ListProduct = listProduct;
            return View(product);
        }
        public ActionResult List(int ProductCategoryID=0, int ageID=0, int page=1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.ageID = ageID;

            PagedList<Product> listProductPaging;
            if (ProductCategoryID==0 && ageID != 0)
            {
                IEnumerable<Product> products = _productService.GetProductList().Where(x => x.AgeID == ageID);
                listProductPaging = new PagedList<Product>(products, page, 12);

                Ages ages = _ageService.GetAgeByID(ageID);
                ViewBag.ageName = ages.Name;
            }
            else if (ProductCategoryID != 0 && ageID == 0 )
            {
                IEnumerable<Product> products = _productService.GetProductListWithCategory(ProductCategoryID);
                listProductPaging = new PagedList<Product>(products, page, 12);

                var productCategoryName = _productCategoryService.GetProductCategoryList().Single(x => x.ID == ProductCategoryID);
                ViewBag.productCategoryName = productCategoryName.Name;
            }
            else
            {
                IEnumerable<Product> products = _productService.GetProductListWithCategory(ProductCategoryID).Where(x => x.AgeID == ageID);
                listProductPaging = new PagedList<Product>(products, page, 12);
            }
            return View(listProductPaging);
        }
    }
}