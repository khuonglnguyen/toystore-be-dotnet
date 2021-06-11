﻿using PagedList;
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
    public class SupplierManageController : Controller
    {
        private ISupplierService _supplierService;
        public SupplierManageController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        // GET: SupplierManage
        public ActionResult Index(int page = 1, string keyword = "")
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            int pageSize = 10;
            if (keyword != "")
            {
                //Get supplier
                var suppliers = _supplierService.GetSupplierList().Where(x=>x.Name.Contains(keyword)).OrderByDescending(x => x.LastUpdatedDate.Date);
                PagedList<Supplier> listProducer = new PagedList<Supplier>(suppliers, page, pageSize);
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
                //Get supplier
                var suppliers = _supplierService.GetSupplierList();
                PagedList<Supplier> listProducer = new PagedList<Supplier>(suppliers, page, pageSize);
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
            List<string> names = _supplierService.GetSupplierListName(Prefix).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            _supplierService.AddSupplier(supplier);
            TempData["create"] = "success";
            //Return view
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get producer
            var supplier = _supplierService.GetByID(id);
            //Check null
            if (supplier != null)
            {
                //Return view
                return Json(new
                {
                    ID = supplier.ID,
                    Name = supplier.Name,
                    Address = supplier.Address,
                    Phone = supplier.Phone,
                    Email = supplier.Email,
                    TotalAmount = supplier.TotalAmount,
                    IsActive = supplier.IsActive,
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
        public ActionResult Edit(Supplier supplier, int page)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Update supplier
            _supplierService.UpdateSupplier(supplier);
            TempData["edit"] = "success";
            //Return view
            return RedirectToAction("Index", new { page = page });
        }
        public void Block(int id)
        {
            //Block supplier
            _supplierService.Block(id);
        }
        public void Active(int id)
        {
            //Active supplier
            _supplierService.Active(id);
        }
    }
}