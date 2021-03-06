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

        public HomeController(IProductCategoryService productCategoryService, IProductService productService, IProducerService producerService, IAgeService ageService, IProductCategoryParentService productCategoryParentService, GenderService genderService, MemberService memberService)
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
            var ProductCategory1 = _productCategoryService.GetProductCategoryByName("Đồ chơi lắp ráp");
            //Get list product 1
            var listProdudct1 = _productService.GetProductListForHomePage(ProductCategory1.ID);
            ViewBag.ListProduct1 = listProdudct1;

            var ProductCategory2 = _productCategoryService.GetProductCategoryByName("Đồ chơi Robot");
            //Get list product 2
            var listProdudct2 = _productService.GetProductListForHomePage(ProductCategory2.ID);
            ViewBag.ListProduct2 = listProdudct2;

            var ProductCategory3 = _productCategoryService.GetProductCategoryByName("Đồ chơi trí tuệ");
            //Get list product 3
            var listProdudct3 = _productService.GetProductListForHomePage(ProductCategory3.ID);
            ViewBag.ListProduct3 = listProdudct3;

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
        [HttpPost]
        public ActionResult SignUp(Member member)
        {
            if (member == null)
            {
                return null;
            }
            else
            {
                _memberService.AddMember(member);
            }
            return RedirectToAction("Index");
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
                Member memberCheck = _memberService.CheckLogin(member.Username, member.Password);
                if (memberCheck != null)
                {
                    Session["Member"] = memberCheck;
                }
                else
                {

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