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
    [Authorize(Roles = "ProductManage")]
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
            _productService = productService;
            _productCategoryService = productCategoryService;
            _supplierService = supplierService;
            _producerService = producerService;
            _ageService = ageService;
            _genderService = genderService;
        }
        #endregion
        // GET: Product
        [HttpGet]
        public ActionResult Index(int page = 1, string keyword = "")
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.CategoryID = new SelectList(_productCategoryService.GetProductCategoryList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.SupplierID = new SelectList(_supplierService.GetSupplierList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ProducerID = new SelectList(_producerService.GetProducerList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.AgeID = new SelectList(_ageService.GetAgeList(), "ID", "Name");
            ViewBag.GenderID = new SelectList(_genderService.GetGenderList(), "ID", "Name");

            ViewBag.CategoryIDEdit = ViewBag.CategoryID;
            ViewBag.SupplierIDEdit = ViewBag.SupplierID;
            ViewBag.ProducerIDEdit = ViewBag.ProducerID;
            ViewBag.AgeIDEdit = ViewBag.AgeID;
            ViewBag.GenderIDEdit = ViewBag.GenderID;

            ViewBag.CategoryIDDetail = ViewBag.CategoryID;
            ViewBag.SupplierIDDetail = ViewBag.SupplierID;
            ViewBag.ProducerIDDetail = ViewBag.ProducerID;
            ViewBag.AgeIDDetail = ViewBag.AgeID;
            ViewBag.GenderIDDetail = ViewBag.GenderID;

            int pageSize = 5;
            //Get proudct category list
            if (keyword != "")
            {
                var products = _productService.GetProductListForManage().Where(x=>x.Name.Contains(keyword)).OrderByDescending(x => x.LastUpdatedDate.Date);
                ViewBag.Products = products;
                PagedList<Product> listProduct = new PagedList<Product>(products, page, pageSize);
                ViewBag.KeyWord = keyword;
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
            else
            {
                var products = _productService.GetProductListForManage().OrderByDescending(x => x.LastUpdatedDate.Date);
                ViewBag.Products = products;
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
        }
        [HttpPost]
        public JsonResult ListName(string Prefix)
        {
            List<string> names = _productService.GetProductListName(Prefix).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult List(string keyword)
        {
            if (Session["User"] == null)
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
                ViewBag.message = "Hiển thị kết quả tìm kiếm với '" + keyword + "";
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
            if (Session["User"] == null)
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
            if (Session["User"] == null)
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
            return RedirectToAction("Index", new { page = 1 });
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get product catetgory
            var product = _productService.GetByID(id);

            //Get data for DropdownList
            ViewBag.CategoryIDEdit = new SelectList(_productCategoryService.GetProductCategoryList().OrderBy(x => x.Name), "ID", "Name", product.CategoryID);
            ViewBag.ProducerIDEdit = new SelectList(_producerService.GetProducerList().OrderBy(x => x.Name), "ID", "Name", product.ProducerID);
            ViewBag.AgeIDEdit = new SelectList(_ageService.GetAgeList(), "ID", "Name", product.AgeID);
            ViewBag.GenderIDEdit = new SelectList(_genderService.GetGenderList(), "ID", "Name", product.GenderID);

            //Check null
            if (product != null)
            {
                //Return view
                return Json(new
                {
                    ID = product.ID,
                    Name = product.Name,
                    CategoryID = product.CategoryID,
                    ProducerID = product.ProducerID,
                    AgeID = product.AgeID,
                    GenderID = product.GenderID,
                    Image1 = product.Image1,
                    Image2 = product.Image2,
                    Image3 = product.Image3,
                    Price = product.Price,
                    Discount = product.Discount,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    ViewCount = product.ViewCount,
                    PurchasedCount = product.PurchasedCount,
                    Quantity = product.Quantity,
                    SEOKeyword = product.SEOKeyword,
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
        public ActionResult Edit(Product product, HttpPostedFileBase[] ImageUpload, int page, int CategoryIDEdit, int ProducerIDEdit, int AgeIDEdit, int GenderIDEdit)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.CategoryID = new SelectList(_productCategoryService.GetProductCategoryList().OrderBy(x => x.Name), "ID", "Name", product.CategoryID);
            ViewBag.ProducerID = new SelectList(_producerService.GetProducerList().OrderBy(x => x.Name), "ID", "Name", product.ProducerID);
            ViewBag.AgeID = new SelectList(_ageService.GetAgeList(), "ID", "Name", product.AgeID);
            ViewBag.GenderID = new SelectList(_genderService.GetGenderList(), "ID", "Name", product.GenderID);

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
            product.CategoryID = CategoryIDEdit;
            product.ProducerID = ProducerIDEdit;
            product.AgeID = AgeIDEdit;
            product.GenderID = GenderIDEdit;
            _productService.UpdateProduct(product);
            string Url = Request.Url.ToString();
            return RedirectToAction("Index", new { page = page });
        }
        public void Block(int id)
        {
            //Get productCategory by ID
            var product = _productService.GetByID(id);
            //Delete productCategory
            _productService.DeleteProduct(product);
        }
        public void Active(int id)
        {
            //Get productCategory by ID
            var product = _productService.GetByID(id);
            //Active productCategory
            _productService.ActiveProduct(product);
        }
        public ActionResult ProductActivePartial(int ID)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            return PartialView("ProductActivePartial", _productService.GetByID(ID));
        }
        [HttpPost]
        public JsonResult CheckName(string name, int id = 0)
        {
            Product product = _productService.GetByName(name);
            if (product != null)
            {
                if (product.ID == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_productService.CheckName(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_productService.CheckName(name))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}