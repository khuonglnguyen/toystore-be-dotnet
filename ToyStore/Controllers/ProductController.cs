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
        private IProductCategoryParentService _productCategoryParentService;
        private IGenderService _genderService;

        public ProductController(IProductService productService, IProducerService producerService, ISupplierService supplierService, IProductCategoryService productCategoryService, IAgeService ageService, IProductCategoryParentService productCategoryParentService,IGenderService genderService)
        {
            this._productService = productService;
            this._producerService = producerService;
            this._supplierService = supplierService;
            this._productCategoryService = productCategoryService;
            this._ageService = ageService;
            this._productCategoryParentService = productCategoryParentService;
            this._genderService = genderService;
        }
        #endregion
        // GET: Product
        public ActionResult Details(int ID)
        {
            var product = _productService.GetByID(ID);
            var producer = _producerService.GetByID(product.ProducerID);
            var supplier = _supplierService.GetByID(product.SupplierID);
            var listProduct = _productService.GetProductListRandom();
            ViewBag.ProducerName = producer.Name;
            ViewBag.SupplierName = supplier.Name;
            ViewBag.ListProduct = listProduct;
            return View(product);
        }
        public ActionResult Ages(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.ageID = ID;
            Ages ages = _ageService.GetAgeByID(ID);
            ViewBag.Name = "Độ tuổi " + ages.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByAge(ID);
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult Producer(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.producerID = ID;
            Producer producer = _producerService.GetByID(ID);
            ViewBag.Name = "Thương hiệu " + producer.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByProducer(ID);
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult ProductCategory(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.productCategoryID = ID;
            ProductCategory productCategory = _productCategoryService.GetByID(ID);
            ViewBag.Name = "Danh mục " + productCategory.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByCategory(ID);
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult ProductCategoryParent(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.productCategoryParentID = ID;
            ProductCategoryParent productCategoryParent = _productCategoryParentService.GetByID(ID);
            ViewBag.Name = "Danh mục gốc " + productCategoryParent.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByCategoryParent(ID);
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult Gender(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.GenderID = ID;
            Gender gender = _genderService.GetGenderByID(ID);
            ViewBag.Name = "Giới tính " + gender.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products;
            if (ID!=3)
            {
                products = _productService.GetProductListByGender(ID);
            }
            else
            {
                products = _productService.GetProductList();
            }
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult NewProduct(int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListIsNew();
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
    }
}