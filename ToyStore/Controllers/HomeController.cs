using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Service;
using ToyStore.Data;
using ToyStore.Data.Repository;
using ToyStore.Models;
using System.Net.Mail;

namespace ToyStore.Controllers
{
    public class HomeController : Controller
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;
        private IProductService _productService;
        private IProducerService _producerService;
        private IAgeService _ageService;
        private IProductCategoryParentService _productCategoryParentService;
        private IGenderService _genderService;
        private IMemberService _memberService;

        public HomeController(IProductCategoryService productCategoryService, IProductService productService, IProducerService producerService, IAgeService ageService, IProductCategoryParentService productCategoryParentService, IGenderService genderService, IMemberService memberService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
            _producerService = producerService;
            _ageService = ageService;
            _productCategoryParentService = productCategoryParentService;
            _genderService = genderService;
            _memberService = memberService;
        }
        #endregion
        public ActionResult Index()
        {
            //Get list product New
            var listProdudctNew = _productService.GetProductListIsNew();
            ViewBag.ListProductNew = listProdudctNew;
            //Get list product 4
            var listProdudct4 = _productService.GetProductListForDiscount();
            ViewBag.ListProduct4 = listProdudct4;

            return View();
        }
        public ActionResult HeaderTopPartial()
        {
            return PartialView();
        }
        public ActionResult MenuPartial()
        {
            ViewBag.ListProductCategory = _productCategoryService.GetProductCategoryList();
            ViewBag.ListProducer = _producerService.GetProducerList();
            ViewBag.ListAge = _ageService.GetAgeList();
            ViewBag.ListParent = _productCategoryParentService.GetProductCategoryParentList();
            ViewBag.ListGender = _genderService.GetGenderList();
            return PartialView();
        }
        public ViewResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Member member)
        {
            if (ModelState.IsValid)
            {
                if (member == null)
                {
                    return null;
                }
                else
                {
                    Member member1 = _memberService.AddMember(member);
                    return RedirectToAction("ConfirmEmail", "Member", new { ID = member1.ID });
                }
            }
            return null;
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Member member)
        {
            if (member == null)
            {
                return null;
            }
            else
            {
                Member memberCheck = _memberService.CheckLogin(member.Email, member.Password);
                if (memberCheck != null)
                {
                    Session["Member"] = memberCheck;
                    if (memberCheck.EmailConfirmed == false)
                    {
                        return RedirectToAction("ConfirmEmail", "Member", new { ID = memberCheck.ID });
                    }
                }
                else
                {
                    ViewBag.Message = "Tên đăng nhập/Email hoặc mật khẩu không đúng.";
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult SignOut()
        {
            Session["Member"] = null;
            return RedirectToAction("Index");
        }
    }
}