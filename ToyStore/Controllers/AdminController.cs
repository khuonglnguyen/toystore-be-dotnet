using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class AdminController : Controller
    {
        private IEmloyeeService _emloyeeService;
        private IEmloyeeTypeService _emloyeeTypeService;
        public AdminController(IEmloyeeService emloyeeService, IEmloyeeTypeService emloyeeTypeService)
        {
            _emloyeeService = emloyeeService;
            _emloyeeTypeService = emloyeeTypeService;
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
                Session["Emloyee"] = emloyeeCheck;
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["Emloyee"] = null;
            return RedirectToAction("Login");
        }
    }
}