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
    public class ProductManageController : Controller
    {
        #region Initialize
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private ISupplierService _supplierService;
        private IProducerService _producerService;
        private IAgeService _ageService;
        private IGenderService _genderService;

        public ProductManageController(IProductService productService, 
            IProductCategoryService productCategoryService, 
            SupplierService supplierService, 
            ProducerService producerService,
            AgeService ageService,
            GenderService genderService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            this._supplierService = supplierService;
            this._producerService = producerService;
            this._ageService = ageService;
            this._genderService = genderService;
        }
        #endregion
        // GET: Product
        [HttpGet]
        public ActionResult List(int page = 1)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.CategoryID = new SelectList(_productCategoryService.GetProductCategoryList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.SupplierID = new SelectList(_supplierService.GetSupplierList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ProducerID = new SelectList(_producerService.GetProducerList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.AgeID = new SelectList(_ageService.GetAgeList(), "ID", "Name");
            ViewBag.GenderID = new SelectList(_genderService.GetGenderList(), "ID", "Name");

            int pageSize = 5;
            //Get proudct category list
            var products = _productService.GetProductListForManage().OrderBy(x=>x.Name);
            PagedList<Product> listProduct = new PagedList<Product>(products, page, pageSize);
            //Check null
            if (listProduct != null)
            {
                ViewBag.Page = page;
                //Return view
                return View(listProduct);
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
            var listProductListName = _productService.GetProductListName(Prefix).ToList();
            return Json(listProductListName, JsonRequestBehavior.AllowGet);
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
            var products = _productService.GetProductListForManage(keyword);
            PagedList<Product> listProduct = new PagedList<Product>(products, 1, pageSize);
            //Check null
            if (listProduct != null)
            {
                ViewBag.message="Hiển thị kết quả tìm kiếm với '"+keyword+"";
                //Return view
                return View(listProduct);
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
            ViewBag.CategoryID = new SelectList(_productCategoryService.GetProductCategoryList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.SupplierID = new SelectList(_supplierService.GetSupplierList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ProducerID = new SelectList(_producerService.GetProducerList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.AgeID = new SelectList(_ageService.GetAgeList(), "ID", "Name");
            ViewBag.GenderID = new SelectList(_genderService.GetGenderList(), "ID", "Name");
            //Return view
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase[] ImageUpload, int page)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Declare a errorCount
            int errorCount = 0;
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                //Check content image
                if (ImageUpload[i] != null && ImageUpload[i].ContentLength > 0)
                {
                    //Check format iamge
                    if (ImageUpload[i].ContentType != "image/jpeg" && ImageUpload[i].ContentType != "image/png" && ImageUpload[i].ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(ImageUpload[i].FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            ImageUpload[i].SaveAs(path);
                        }
                    }
                }
            }
            //Set new value image for product
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                if (ImageUpload[i] != null)
                {
                    if (i == 0)
                        product.Image1 = ImageUpload[0].FileName;
                    else if (i == 1)
                        product.Image2 = ImageUpload[1].FileName;
                    else if (i == 2)
                        product.Image3 = ImageUpload[2].FileName;
                    else if (i == 3)
                        product.Image4 = ImageUpload[3].FileName;
                }
            }
            //Set TempData for checking in view to show swal
            TempData["create"] = "Success";
            //Create productCategory
            _productService.AddProduct(product);
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
            var product = _productService.GetByID(id);

            //Get data for DropdownList
            ViewBag.CategoryID = new SelectList(_productCategoryService.GetProductCategoryList().OrderBy(x => x.Name), "ID", "Name",product.CategoryID);
            ViewBag.SupplierID = new SelectList(_supplierService.GetSupplierList().OrderBy(x => x.Name), "ID", "Name", product.SupplierID);
            ViewBag.ProducerID = new SelectList(_producerService.GetProducerList().OrderBy(x => x.Name), "ID", "Name", product.ProducerID);
            ViewBag.AgeID = new SelectList(_ageService.GetAgeList(), "ID", "Name", product.AgeID);
            ViewBag.GenderID = new SelectList(_genderService.GetGenderList(), "ID", "Name");

            //Check null
            if (product != null)
            {
                //Return view
                return Json(new
                {
                    data = product,
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
        public ActionResult Edit(Product product, HttpPostedFileBase[] ImageUpload, int page)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.CategoryID = new SelectList(_productCategoryService.GetProductCategoryList().OrderBy(x => x.Name), "ID", "Name", product.CategoryID);
            ViewBag.SupplierID = new SelectList(_supplierService.GetSupplierList().OrderBy(x => x.Name), "ID", "Name", product.SupplierID);
            ViewBag.ProducerID = new SelectList(_producerService.GetProducerList().OrderBy(x => x.Name), "ID", "Name", product.ProducerID);
            ViewBag.AgeID = new SelectList(_ageService.GetAgeList(), "ID", "Name", product.AgeID);

            //Declare a errorCount
            int errorCount = 0;
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                //Check content image
                if (ImageUpload[i] != null && ImageUpload[i].ContentLength > 0)
                {
                    //Check format iamge
                    if (ImageUpload[i].ContentType != "image/jpeg" && ImageUpload[i].ContentType != "image/png" && ImageUpload[i].ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(ImageUpload[i].FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            ImageUpload[i].SaveAs(path);
                        }
                    }
                }
            }
            //Set new value image for product
            for (int i = 0; i < ImageUpload.Length; i++)
            {
                if (ImageUpload[i] != null)
                {
                    if (i == 0)
                        product.Image1 = ImageUpload[0].FileName;
                    else if (i == 1)
                        product.Image2 = ImageUpload[1].FileName;
                    else if (i == 2)
                        product.Image3 = ImageUpload[2].FileName;
                    else if (i == 3)
                        product.Image4 = ImageUpload[3].FileName;
                }
            }

            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update productCategory
            _productService.UpdateProduct(product);
            string Url = Request.Url.ToString();
            return RedirectToAction("List",new { page=page});
        }
        [HttpGet]
        public ActionResult Block(int id, int row)
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
            var product = _productService.GetByID(id);
            //Check null
            if (product == null)
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Delete productCategory
            _productService.DeleteProduct(product);
            //Set TempData for checking in view to show swal
            TempData["block"] = "Success";
            ViewBag.Row = row;
            return PartialView("ProductActivePartial",_productService.GetByID(id));
        }
        [HttpGet]
        public ActionResult Active(int id, int row)
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
            var product = _productService.GetByID(id);
            //Check null
            if (product == null)
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Active productCategory
            _productService.ActiveProduct(product);
            //Set TempData for checking in view to show swal
            TempData["acitve"] = "Success";
            //Return RedirectToAction
            ViewBag.Row = row;
            return PartialView("ProductActivePartial", _productService.GetByID(id));
        }
        [HttpPost]
        public ActionResult DeleteMulti(FormCollection formCollection)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            string[] Ids = formCollection["IDDelete"].Split(new char[] { ',' });
            _productService.MultiDeleteProduct(Ids);
            //Set TempData for checking in view to show swal
            TempData["deleteMulti"] = "Success";
            return RedirectToAction("List");
        }
        public ActionResult ProductActivePartial(int ID)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            return PartialView("ProductActivePartial", _productService.GetByID(ID));
        }
    }
}