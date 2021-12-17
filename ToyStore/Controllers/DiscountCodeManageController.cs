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
    [Authorize(Roles = "DiscountCodeManage")]
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
            User user = Session["User"] as User;
            discountCode.UserID = user.ID;
            _discountCodeService.AddDiscountCode(discountCode, Quantity);
            TempData["create"] = "success";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public void Block(int id)
        {
            _discountCodeService.Block(id);
        }
        [HttpGet]
        public void Active(int id)
        {
            _discountCodeService.Active(id);
        }
    }
}