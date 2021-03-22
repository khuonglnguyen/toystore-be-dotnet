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
    public class QAManageController : Controller
    {
        private IQAService _qAService;
        private IProductService _productService;
        private IMemberService _memberService;
        public QAManageController(IQAService qAService, IProductService productService, IMemberService memberService)
        {
            _qAService = qAService;
            _productService = productService;
            _memberService = memberService;
        }
        // GET: QAManage
        public ActionResult List(int page = 1)
        {
            int pageSize = 5;
            //Get qAs list
            var qAs = _qAService.GetQAList().OrderBy(x => x.DateQuestion);
            PagedList<QA> listqAs = new PagedList<QA>(qAs, page, pageSize);
            //Check null
            if (listqAs != null)
            {
                ViewBag.Page = page;
                ViewBag.ListMember = _memberService.GetMemberList();
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
                data = qAs,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(QA qA, string DateQuestion, int page)
        {
            qA.DateQuestion = DateTime.Parse(DateQuestion);
            _qAService.UpdateQA(qA);
            return RedirectToAction("List", new { page = page });
        }
        [HttpPost]
        public ActionResult Answer(QA qA, string DateQuestion, int page)
        {
            qA.DateQuestion = DateTime.Parse(DateQuestion);
            _qAService.UpdateQA(qA);
            return RedirectToAction("List", new { page = page });
        }
    }
}