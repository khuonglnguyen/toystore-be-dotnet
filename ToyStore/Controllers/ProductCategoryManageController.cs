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
    [Authorize(Roles = "ProductCategoryManage")]
    public class ProductCategoryManageController : Controller
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;
        private IProductCategoryParentService _productCategoryParentService;

        public ProductCategoryManageController(IProductCategoryService productCategoryService, ProductCategoryParentService productCategoryParentService)
        {
            this._productCategoryService = productCategoryService;
            this._productCategoryParentService = productCategoryParentService;
        }
        #endregion

        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.ParentIDEdit = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ParentID = ViewBag.ParentIDEdit;
            int pageSize = 10;
            //Get proudct category list
            var productCategories = _productCategoryService.GetProductCategoryList();
            PagedList<ProductCategory> listProductCategory = new PagedList<ProductCategory>(productCategories, page, pageSize);
            //Check null
            if (listProductCategory != null)
            {
                ViewBag.Page = page;
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
            List<string> names = _productCategoryService.GetProductCategoryListName(Prefix);
            return Json(names, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Search(string keyword, int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.ParentIDEdit = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name");
            ViewBag.ParentID = ViewBag.ParentIDEdit;
            int pageSize = 10;
            //Get proudct category list
            var productCategories = _productCategoryService.GetProductCategoryList().Where(x => x.Name.Contains(keyword));
            PagedList<ProductCategory> listProductCategory = new PagedList<ProductCategory>(productCategories, page, pageSize);
            //Check null
            if (listProductCategory != null)
            {
                ViewBag.KeyWord = keyword;
                ViewBag.Page = page;
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
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.ParentID = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name");
            //Return view
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory, int page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Set TempData for checking in view to show swal
            TempData["create"] = "Success";
            //Create productCategory
            _productCategoryService.AddProductCategory(productCategory);
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
            var productCategory = _productCategoryService.GetByID(id);

            //Get data for DropdownList
            ViewBag.ParentIDEdit = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name", productCategory.ParentID);

            //Check null
            if (productCategory != null)
            {
                //Return view
                return Json(new
                {
                    ID = productCategory.ID,
                    Name = productCategory.Name,
                    ParentID = productCategory.ParentID,
                    IsActive = productCategory.IsActive,
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
        public ActionResult Edit(ProductCategory productCategory, int page, int ParentIDEdit)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update productCategory
            productCategory.ParentID = ParentIDEdit;
            _productCategoryService.UpdateProductCategory(productCategory);
            //Return view
            return RedirectToAction("Index", new { page = page });
        }
        public void Block(int id)
        {
            //Get productCategory by ID
            var productCategory = _productCategoryService.GetByID(id);
            //Block productCategory
            _productCategoryService.Block(productCategory);
            //Set TempData for checking in view to show swal
            TempData["delete"] = "Success";
        }
        public void Active(int id)
        {
            //Get productCategory by ID
            var productCategory = _productCategoryService.GetByID(id);
            //Block productCategory
            _productCategoryService.Active(productCategory);
            //Set TempData for checking in view to show swal
            TempData["delete"] = "Success";
        }
        [HttpPost]
        public ActionResult DeleteMulti(FormCollection formCollection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            string[] Ids = formCollection["IDDelete"].Split(new char[] { ',' });
            _productCategoryService.MultiDeleteProductCategory(Ids);
            //Set TempData for checking in view to show swal
            TempData["deleteMulti"] = "Success";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult CheckName(string name, int id = 0)
        {
            ProductCategory productCategory = _productCategoryService.GetByName(name);
            if (productCategory != null)
            {
                if (productCategory.ID == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_productCategoryService.CheckName(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_productCategoryService.CheckName(name))
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