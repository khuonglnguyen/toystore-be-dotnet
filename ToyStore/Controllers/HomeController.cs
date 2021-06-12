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
using System.Web.Security;

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
        private ICartService _cartService;

        public HomeController(IProductCategoryService productCategoryService, IProductService productService, IProducerService producerService, IAgeService ageService, IProductCategoryParentService productCategoryParentService, IGenderService genderService, IMemberService memberService, ICartService cartService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
            _producerService = producerService;
            _ageService = ageService;
            _productCategoryParentService = productCategoryParentService;
            _genderService = genderService;
            _memberService = memberService;
            _cartService = cartService;
        }
        #endregion
        public ActionResult Index()
        {
            //Get list product New
            var listProdudctNew = _productService.GetProductListIsNew();
            ViewBag.ListProductNew = listProdudctNew;
            var listProdudct = _productService.GetProductListIndex();
            ViewBag.listProduct = listProdudct;

            return View();
        }
        public ActionResult HeaderTopPartial()
        {
            return PartialView();
        }
        public ActionResult MenuPartial()
        {
            ViewBag.ListProductCategory = _productCategoryService.GetProductCategoryHome();
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
            bool fail = false;
            //Check email
            if (_memberService.CheckEmail(member.Email) == false)
            {
                ViewBag.MessageEmail = "Email đã tồn tại";
                fail = true;
            }
            //Check username
            if (_memberService.CheckUsername(member.Username) == false)
            {
                ViewBag.MessageUsername = "Username đã tồn tại";
                fail = true;
            }
            //Check phonenumber
            if (_memberService.CheckPhoneNumber(member.PhoneNumber) == false)
            {
                ViewBag.MessagePhoneNumber = "Số điện thoại đã tồn tại";
                fail = true;
            }
            if (fail)
            {
                return View(member);
            }
            else
            {
                Member member1 = _memberService.AddMember(member);
                return RedirectToAction("ConfirmEmail", "Member", new { ID = member1.ID });
            }
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
                    else
                    {
                        if (_cartService.CheckCartMember(memberCheck.ID))
                        {
                            List<ItemCart> carts = _cartService.GetCart(memberCheck.ID);
                            Session["Cart"] = carts;
                            return RedirectToAction("Index");
                        }
                        if (Session["Cart"] != null)
                        {
                            List<ItemCart> listCart = Session["Cart"] as List<ItemCart>;
                            foreach (var item in listCart)
                            {
                                item.MemberID = memberCheck.ID;
                                _cartService.AddCartIntoMember(item);
                            }
                        }
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
            Session["Cart"] = null;
            return RedirectToAction("Index");
        }
    }
}