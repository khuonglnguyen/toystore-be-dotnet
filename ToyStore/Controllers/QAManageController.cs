using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    [Authorize(Roles = "QAManage")]
    public class QAManageController : Controller
    {
        private IQAService _qAService;
        private IProductService _productService;
        private IMemberService _memberService;
        private IEmloyeeService _emloyeeService;
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
        public ActionResult Edit(QA qA, string DateQuestion, int page)
        {
            qA.DateQuestion = DateTime.Parse(DateQuestion);
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            qA.EmloyeeID = emloyee.ID;
            _qAService.UpdateQA(qA);
            return RedirectToAction("List", new { page = page });
        }
        [HttpPost]
        public ActionResult Answer(QA qA, string DateQuestion, int page)
        {
            qA.DateQuestion = DateTime.Parse(DateQuestion);
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            qA.EmloyeeID = emloyee.ID;
            _qAService.UpdateQA(qA);
            return RedirectToAction("List", new { page = page });
        }
    }
}