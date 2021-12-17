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
    [Authorize(Roles = "ProducerManage")]
    public class ProducerManageController : Controller
    {
        #region Initialize
        private IProducerService _producerService;

        public ProducerManageController(IProducerService producerService)
        {
            _producerService = producerService;
        }
        #endregion
        [HttpGet]
        public ActionResult Index(int page = 1, string keyword = "")
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            if (keyword != "")
            {
                int pageSize = 10;
                //Get proudcer
                var producers = _producerService.GetProducerList().Where(x => x.Name.Contains(keyword));
                PagedList<Producer> listProducer = new PagedList<Producer>(producers, page, pageSize);
                //Check null
                if (listProducer != null)
                {
                    ViewBag.Page = page;
                    ViewBag.KeyWord = keyword;
                    //Return view
                    return View(listProducer);
                }
                else
                {
                    //return 404
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                int pageSize = 10;
                //Get proudcer
                var producers = _producerService.GetProducerList();
                PagedList<Producer> listProducer = new PagedList<Producer>(producers, page, pageSize);
                //Check null
                if (listProducer != null)
                {
                    ViewBag.Page = page;
                    //Return view
                    return View(listProducer);
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
            List<string> names = _producerService.GetProducerListName(Prefix);
            return Json(names, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult List(string keyword)
        //{
        //    if (Session["Emloyee"] == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    //Get data for DropdownList
        //    ViewBag.ParentIDEdit = new SelectList(_productCategoryParentService.GetProductCategoryParentList().OrderBy(x => x.Name), "ID", "Name");
        //    int pageSize = 5;
        //    if (keyword == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    //Get proudct category list with keyword
        //    var ProductCategories = _productCategoryService.GetProductCategoryList(keyword);
        //    PagedList<ProductCategory> listProductCategory = new PagedList<ProductCategory>(ProductCategories, 1, pageSize);
        //    //Check null
        //    if (listProductCategory != null)
        //    {
        //        //Return view
        //        return View(listProductCategory);
        //    }
        //    else
        //    {
        //        //return 404
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //}
        [HttpPost]
        public ActionResult Create(Producer producer, int page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Create producer
            _producerService.AddProducer(producer);
            //Set TempData for checking in view to show swal
            TempData["create"] = "Success";
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
            //Get producer
            var producer = _producerService.GetByID(id);
            //Check null
            if (producer != null)
            {
                //Return view
                return Json(new
                {
                    ID = producer.ID,
                    Name = producer.Name,
                    IsActive = producer.IsActive,
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
        public ActionResult Edit(Producer producer, int page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            //Update producer
            _producerService.UpdateProducer(producer);
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Return view
            return RedirectToAction("Index", new { page = page });
        }
        public void Block(int id)
        {
            //Get producer
            var producer = _producerService.GetByID(id);
            //Block producer
            _producerService.Block(producer);
        }
        public void Active(int id)
        {
            //Get producer
            var producer = _producerService.GetByID(id);
            //Block producer
            _producerService.Active(producer);
        }
        [HttpPost]
        public JsonResult CheckName(string name, int id = 0)
        {
            Producer producer = _producerService.GetByName(name);
            if (producer != null)
            {
                if (producer.ID == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_producerService.CheckName(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_producerService.CheckName(name))
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