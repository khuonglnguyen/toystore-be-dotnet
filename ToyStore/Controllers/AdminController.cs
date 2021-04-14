using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class AdminController : Controller
    {
        private IEmloyeeService _emloyeeService;
        private IEmloyeeTypeService _emloyeeTypeService;
        private IDecentralizationService _decentralizationService;
        private IMemberService _memberService;
        private IProductService _productService;
        private IOrderService _orderService;
        public AdminController(IEmloyeeService emloyeeService, IEmloyeeTypeService emloyeeTypeService, IDecentralizationService decentralizationService, IMemberService memberService, IProductService productService, IOrderService orderService)
        {
            _emloyeeService = emloyeeService;
            _emloyeeTypeService = emloyeeTypeService;
            _decentralizationService = decentralizationService;
            _memberService = memberService;
            _productService = productService;
            _orderService = orderService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.SumAccessTimes = HttpContext.Application["SumAccessTimes"].ToString();
                ViewBag.RealAccessTimes = HttpContext.Application["RealAccessTimes"].ToString();
                Emloyee emloyee = Session["Emloyee"] as Emloyee;
                ViewBag.EmloyeeTypeName = (_emloyeeTypeService.GetEmloyeeTypeByID(emloyee.EmloyeeTypeID)).Name;
                ViewBag.TotalMember = _memberService.GetTotalMember();
                ViewBag.TotalEmloyee = _emloyeeService.GetTotalEmloyee();
                ViewBag.TotalProduct = _productService.GetTotalProduct();
                ViewBag.TotalProductPurchased = _productService.GetTotalProductPurchased();
                decimal TotalRevenue = _orderService.GetTotalRevenue();
                if (TotalRevenue < 1000000)
                {
                    TotalRevenue = TotalRevenue / 1000;
                    ViewBag.TotalRevenue = (int)TotalRevenue+"K";
                }
                else
                {
                    TotalRevenue = TotalRevenue / 1000000;
                    ViewBag.TotalRevenue = (int)TotalRevenue + "M";
                }
                ViewBag.TotalOrder = _orderService.GetTotalOrder();
                return View();
            }
        }
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Emloyee emloyee)
        {
            //Check login
            Emloyee emloyeeCheck = _emloyeeService.CheckLogin(emloyee.Username, emloyee.Password);
            if (emloyeeCheck != null)
            {

                IEnumerable<Decentralization> decentralizations = _decentralizationService.GetDecentralizationByEmloyeeTypeID(emloyeeCheck.EmloyeeTypeID);
                string role = "";
                foreach (var item in decentralizations)
                {
                    role += item.Role.Name + ",";
                }

                role = role.Substring(0, role.Length - 1);
                Decentralization(emloyeeCheck.Username, role);

                Session["Emloyee"] = emloyeeCheck;
                return RedirectToAction("Index");
            }
            return View();
        }
        private void Decentralization(string Username, string Role)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                Username,
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
        [HttpGet]
        public ActionResult Logout()
        {
            Session["Emloyee"] = null;
            return RedirectToAction("Login");
        }
    }
}