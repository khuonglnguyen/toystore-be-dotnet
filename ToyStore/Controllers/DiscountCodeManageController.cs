using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index(int page=1)
        {
            IEnumerable<DiscountCode> discountCodes = _discountCodeService.GetDiscountCodeList();
            PagedList<DiscountCode> discountCodesPaging = new PagedList<DiscountCode>(discountCodes, page, 12);
            return View(discountCodesPaging);
        }
    }
}