using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;
using ToyStore.ListenerModels;
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
                    ViewBag.TotalRevenue = TotalRevenue.ToString("0.##") + "K";
                }
                else
                {
                    TotalRevenue = TotalRevenue / 1000000;
                    ViewBag.TotalRevenue = TotalRevenue.ToString("0.##") + "M";
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
            Emloyee emloyeeCheck = _emloyeeService.CheckLogin(emloyee.ID, emloyee.Password);
            if (emloyeeCheck != null)
            {

                IEnumerable<Decentralization> decentralizations = _decentralizationService.GetDecentralizationByEmloyeeTypeID(emloyeeCheck.EmloyeeTypeID);
                string role = "";
                foreach (var item in decentralizations)
                {
                    role += item.Role.Name + ",";
                }

                role = role.Substring(0, role.Length - 1);
                Decentralization(emloyeeCheck.ID, role);

                Session["Emloyee"] = emloyeeCheck;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Success";
            }
            return View();
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
        [HttpGet]
        public ActionResult Logout()
        {
            Session["Emloyee"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult Incompetent()
        {
            return View();
        }
        [HttpGet]
        public ActionResult InfoEmloyee()
        {
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            return View(emloyee);
        }
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get emloyee
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            //Check null
            if (emloyee != null)
            {
                //Return view
                return Json(new
                {
                    ID = emloyee.ID,
                    FullName = emloyee.FullName,
                    Address = emloyee.Address,
                    Email = emloyee.Email,
                    PhoneNumber = emloyee.PhoneNumber,
                    Image = emloyee.Image,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Edit(Emloyee emloyee, HttpPostedFileBase ImageUpload)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.EmloyeeTypeIDEdit = new SelectList(_emloyeeTypeService.GetListEmloyeeType().OrderBy(x => x.Name), "ID", "Name");

            if (ImageUpload != null)
            {
                int errorCount = 0;
                //Check content image
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    //Check format iamge
                    if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            ImageUpload.SaveAs(path);
                        }
                    }
                }
                //Set new value image for emloyee
                emloyee.Image = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update emloyeetype
            Emloyee e = _emloyeeService.GetByID(emloyee.ID);
            e.FullName = emloyee.FullName;
            e.Address = emloyee.Address;
            e.Email = emloyee.Email;
            e.Image = emloyee.Image;
            _emloyeeService.Update(e);
            Session["Emloyee"] = e;
            string Url = Request.Url.ToString();
            return RedirectToAction("InfoEmloyee");
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string CurrentPassword, string NewPassword)
        {
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            Emloyee emloyeeCheck = _emloyeeService.CheckLogin(emloyee.ID, CurrentPassword);
            if (emloyeeCheck != null)
            {
                _emloyeeService.ResetPassword(emloyeeCheck.ID, NewPassword);
                TempData["ResetPassword"] = "Success";
                return RedirectToAction("InfoEmloyee");
            }
            else
            {
                ViewBag.Message = "Mật khẩu hiện tại không đúng!";
            }
            return View();
        }
    }
}