using PagedList;
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
    public class DiscountCodeManageController : Controller
    {
        private IDiscountCodeService _discountCodeService;
        public DiscountCodeManageController(IDiscountCodeService discountCodeService)
        {
            _discountCodeService = discountCodeService;
        }
        // GET: DiscountCodeManage
        [HttpGet]
        public ActionResult Index(int page=1)
        {
            IEnumerable<DiscountCode> discountCodes = _discountCodeService.GetDiscountCodeList();
            PagedList<DiscountCode> discountCodesPaging = new PagedList<DiscountCode>(discountCodes, page, 12);
            return View(discountCodesPaging);
        }
        [HttpPost]
        public ActionResult Create(DiscountCode discountCode, int Quantity)
        {
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            discountCode.EmloyeeID = emloyee.ID;
            _discountCodeService.AddDiscountCode(discountCode, Quantity);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Block(int id, int row)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Block discountcode
            _discountCodeService.Block(id);
            ViewBag.Row = row;
            return PartialView("DiscountCodePartial", _discountCodeService.GetByID(id));
        }
        [HttpGet]
        public ActionResult Active(int id, int row)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Block discountcode
            _discountCodeService.Active(id);
            ViewBag.Row = row;
            return PartialView("DiscountCodePartial", _discountCodeService.GetByID(id));
        }
    }
}