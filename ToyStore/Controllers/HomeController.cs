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
        private IUserService _userService;
        private ICartService _cartService;
        private IDecentralizationService _decentralizationService;

        public HomeController(IProductCategoryService productCategoryService, IProductService productService, IProducerService producerService, IAgeService ageService, IProductCategoryParentService productCategoryParentService, IGenderService genderService, IUserService userService, ICartService cartService, IDecentralizationService decentralizationService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
            _producerService = producerService;
            _ageService = ageService;
            _productCategoryParentService = productCategoryParentService;
            _genderService = genderService;
            _userService = userService;
            _cartService = cartService;
            _decentralizationService = decentralizationService;
        }
        #endregion
        public ActionResult Index()
        {
            //Get list product New
            var listProdudctNew = _productService.GetProductListLast();
            ViewBag.ListProductNew = listProdudctNew;
            var listProdudct = _productService.GetProductListIndex();
            ViewBag.listProduct = listProdudct;
            var listProdudctDisocunt = _productService.GetProductListDiscount();
            ViewBag.listProductDiscount = listProdudctDisocunt;

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
        public ActionResult SignUp(User user)
        {
            Models.User check = _userService.GetByEmail(user.Email);
            if (check == null)
            {
                Models.User check2 = _userService.GetByPhoneNumber(user.PhoneNumber);
                if (check2 != null)
                {
                    check2.FullName = user.FullName;
                    check2.Address = user.Address;
                    check2.Password = user.Password;
                    check2.PhoneNumber = user.PhoneNumber;
                    _userService.Update(check2);
                    return RedirectToAction("ConfirmEmail", "User", new { ID = check2.ID });
                }
            }
            else if (check != null && check.EmailConfirmed == false)
            {
                check.FullName = user.FullName;
                check.Address = user.Address;
                check.Password = user.Password;
                check.PhoneNumber = user.PhoneNumber;
                _userService.Update(check);
                return RedirectToAction("ConfirmEmail", "User", new { ID = check.ID });
            }

            bool fail = false;
            //Check email
            if (_userService.CheckEmail(user.Email) == false)
            {
                ViewBag.MessageEmail = "Email đã tồn tại";
                fail = true;
            }
            //Check phonenumber
            if (_userService.CheckPhoneNumber(user.PhoneNumber) == false)
            {
                ViewBag.MessagePhoneNumber = "Số điện thoại đã tồn tại";
                fail = true;
            }
            if (fail)
            {
                return View(user);
            }
            else
            {
                User user1 = _userService.Add(user);
                return RedirectToAction("ConfirmEmail", "User", new { ID = user1.ID });
            }
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(User user)
        {
            if (user == null)
            {
                return null;
            }
            else
            {
                User userCheck = _userService.CheckLogin(user.Email, user.Password);
                if (userCheck != null)
                {
                    if (userCheck.EmailConfirmed == false)
                    {
                        return RedirectToAction("ConfirmEmail", "User", new { ID = userCheck.ID });
                    }
                    else
                    {
                        Session["User"] = userCheck;
                        IEnumerable<Decentralization> decentralizations = _decentralizationService.GetDecentralizationByUserTypeID(userCheck.UserTypeID);
                        if (decentralizations.Count() > 0)
                        {
                            string role = "";
                            foreach (var item in decentralizations)
                            {
                                role += item.Role.Name + ",";
                            }

                            role = role.Substring(0, role.Length - 1);
                            Decentralization(userCheck.ID, role);
                        }
                        

                        if (_cartService.CheckCartUser(userCheck.ID))
                        {
                            List<ItemCart> carts = _cartService.GetCart(userCheck.ID);
                            Session["Cart"] = carts;
                            return RedirectToAction("Index");
                        }
                        if (Session["Cart"] != null)
                        {
                            List<ItemCart> listCart = Session["Cart"] as List<ItemCart>;
                            foreach (var item in listCart)
                            {
                                item.UserID = userCheck.ID;
                                _cartService.AddCartIntoUser(item);
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "Email hoặc mật khẩu không đúng.";
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        private void Decentralization(int ID, string Role)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                ID.ToString(),
                DateTime.Now,
                DateTime.Now.AddHours(3),
                false,
                Role,
                FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            Response.Cookies.Add(cookie);
        }

        public ActionResult SignOut()
        {
            Session.Remove("User");
            Session.Remove("Cart");
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index");
        }
    }
}