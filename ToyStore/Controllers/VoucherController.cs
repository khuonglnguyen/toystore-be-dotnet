using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class VoucherController : Controller
    {
        private DiscountCodeDetailService _discountCodeDetailService;
        public VoucherController(DiscountCodeDetailService discountCodeDetailService)
        {
            _discountCodeDetailService = discountCodeDetailService;
        }
        // GET: Voucher
        public ActionResult Index()
        {
            Models.User user = Session["User"] as Models.User;
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var list = _discountCodeDetailService.GetDiscountCodeDetailListByUser(user.ID);
            return View(list);
        }
    }
}