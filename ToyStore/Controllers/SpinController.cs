using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Service;
using ToyStore.Models;

namespace ToyStore.Controllers
{
    public class SpinController : Controller
    {
        private UsersSpinService _usersSpinService;
        private DiscountCodeService _discountCodeService;
        private DiscountCodeDetailService _discountCodeDetailService;
        private UserDiscountCodeService _userDiscountCodeService;
        public SpinController(UsersSpinService usersSpinService, DiscountCodeService discountCodeService, DiscountCodeDetailService discountCodeDetailService, UserDiscountCodeService userDiscountCodeService)
        {
            _usersSpinService = usersSpinService;
            _discountCodeService = discountCodeService;
            _discountCodeDetailService = discountCodeDetailService;
            _userDiscountCodeService = userDiscountCodeService;
        }
        // GET: Spin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetNumberOfSpin()
        {
            Models.User user = Session["User"] as Models.User;
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int result = _usersSpinService.GetNumberOfSpinsByUserID(user.ID);
            return Json(new { num = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetVoucher()
        {
            var result = _discountCodeService.GetDiscountCodeList().OrderBy(x=>x.NumberDiscount).Select(x=>new { NumberDiscount = x.NumberDiscount});
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddVoucher(int NumberDiscount)
        {
            Models.User user = Session["User"] as Models.User;
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string code = _discountCodeDetailService.GetCodeFirstByNumberDiscount(NumberDiscount);
            if (_userDiscountCodeService.GiftSpin(user.ID, code))
            {
                //Update SubOnceSpin
                if (_usersSpinService.SubOnceSpin(user.ID))
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}