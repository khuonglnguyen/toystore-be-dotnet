﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;
using System.Data.Entity;

namespace ToyStore.Controllers
{
    [Authorize(Roles = "QAManage")]
    public class QAManageController : Controller
    {
        private IQAService _qAService;
        private IProductService _productService;
        private IMemberService _memberService;
        private IEmloyeeService _emloyeeService;
        ToyStore2021Entities context = new ToyStore2021Entities();
        public QAManageController(IQAService qAService, IProductService productService, IMemberService memberService, IEmloyeeService emloyeeService)
        {
            _qAService = qAService;
            _productService = productService;
            _memberService = memberService;
            _emloyeeService = emloyeeService;
        }
        // GET: QAManage
        public ActionResult List(int page = 1)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageSize = 5;
            //Get qAs list
            IEnumerable<QA> qAs = _qAService.GetQAList().OrderBy(x => x.DateQuestion);
            PagedList<QA> listqAs = new PagedList<QA>(qAs, page, pageSize);
            //Check null
            if (listqAs != null)
            {
                ViewBag.Page = page;
                //Return view
                return View(listqAs);
            }
            else
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return null;
            }
            //Get product catetgory
            var qAs = _qAService.GetQAByID(id);
            if (qAs == null)
            {
                return null;
            }
            return Json(new
            {
                ID = qAs.ID,
                MemberID = qAs.MemberID,
                ProductID = qAs.ProductID,
                Question = qAs.Question,
                Answer = qAs.Answer,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(QA qA, int page)
        {
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            QA qa = _qAService.GetQAByID(qA.ID);
            qa.EmloyeeID = emloyee.ID;
            qa.Answer = qA.Answer;
            qa.DateAnswer = DateTime.Now;
            _qAService.UpdateQA(qa);
            TempData["ModifiedQA"] = "Success";
            return RedirectToAction("List", new { page = page });
        }
        [HttpGet]
        public ActionResult Delete(int ID, int ProductID)
        {
            _qAService.Delete(ID);
            IEnumerable<QA> listQA = context.QAs.Include(x => x.Emloyee).Include(x => x.Member).Where(x => x.ProductID == ID).OrderByDescending(x => x.DateQuestion).ToList();
            ViewBag.QAList = listQA;
            return RedirectToAction("QAPartial","Product",new { ID=ProductID});
        }
        [HttpGet]
        public ActionResult EditQuestion(QA qA, int ProductID)
        {
            QA qa = _qAService.GetQAByID(qA.ID);
            qa.Question = qA.Question;
            _qAService.UpdateQA(qa);
            return RedirectToAction("QAPartial", "Product", new { ID = ProductID });
        }
    }
}