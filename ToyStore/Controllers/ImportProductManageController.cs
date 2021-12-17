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
    [Authorize(Roles = "ImportProductManage")]
    public class ImportProductManageController : Controller
    {
        private IImportCouponService _importCouponService;
        private IImportCouponDetailService _importCouponDeatilService;
        private ISupplierService _supplierService;
        private IProductService _productService;
        public ImportProductManageController(IImportCouponService importCouponService, IImportCouponDetailService importCouponDetailService, ISupplierService supplierService, IProductService productService)
        {
            _importCouponService = importCouponService;
            _importCouponDeatilService = importCouponDetailService;
            _supplierService = supplierService;
            _productService = productService;
        }
        [HttpGet]
        public ActionResult ImportProduct()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.listSupplier = _supplierService.GetSupplierList();
            return View();
        }
        [HttpGet]
        public ActionResult ImportProductBySupplierID(int ID)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.listSupplier = _supplierService.GetSupplierList();
            ViewBag.SupplierID = ID;
            ViewBag.SupplierName = _supplierService.GetByID(ID).Name;
            ViewBag.ListProduct = _productService.GetProductList().OrderBy(x=>x.Quantity);
            return View();
        }
        [HttpPost]
        public ActionResult ImportProduct(ImportCoupon Model, IEnumerable<ImportCouponDetail> ListModel)
        {
            ViewBag.listSupplier = _supplierService.GetSupplierList();
            ViewBag.ListProduct = _productService.GetProductList();
            User user = Session["User"] as User;
            Model.UserID = user.ID;
            Model.Date = DateTime.Now;
            Model.IsDelete = false;
            //Add import coupon
            _importCouponService.AddImportCoupon(Model);
            //Update quantity product
            Product product;
            foreach (var item in ListModel)
            {
                //Set ImportCouponID for all ImportCouponDetail
                item.ImportCouponID = Model.ID;
                //Update quanitty number
                product = _productService.GetByID(item.ProductID);
                product.Quantity += item.Quantity;
                _productService.UpdateProduct(product);

                _importCouponDeatilService.AddImportCouponDetail(item);
            }
            //Set TempData for checking in view to show swal
            TempData["ImportProduct"] = "Success";
            return View();
        }
        [HttpGet]
        public ActionResult ProductListIsAlmostOver(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.Page = page;
            var LstProduct = _productService.GetProductListAlmostOver();
            PagedList<Product> listProduct = new PagedList<Product>(LstProduct, page, 10);
            return View(listProduct);
        }
        [HttpGet]
        public ActionResult ImportCoupon(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var LstImportCoupon = _importCouponService.GetImportCoupon();
            PagedList<ImportCoupon> listImportCoupon = new PagedList<ImportCoupon>(LstImportCoupon, page, 10);
            ViewBag.Page = page;
            return View(listImportCoupon);
        }
        [HttpGet]
        public ActionResult ImportProductDetail(int ID)
        {
            IEnumerable<ImportCouponDetail> importCouponDetails = _importCouponDeatilService.GetByImportCouponID(ID);
            ViewBag.ID = ID;
            return View(importCouponDetails);
        }
        [HttpGet]
        public ActionResult Delete(int ID, int page)
        {
            _importCouponService.Delete(ID);
            return RedirectToAction("ImportCoupon", new { page = page });
        }
        [HttpGet]
        public ActionResult Rehibilitate(int ID, int page)
        {
            _importCouponService.Rehibilitate(ID);
            return RedirectToAction("ImportCoupon", new { page = page });
        }
    }
}