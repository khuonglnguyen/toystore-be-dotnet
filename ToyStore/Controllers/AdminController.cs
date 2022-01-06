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
    [Authorize(Roles = "AdminHome")]
    public class AdminController : Controller
    {
        private IUserService _userService;
        private IUserTypeService _userTypeService;
        private IDecentralizationService _decentralizationService;
        private IProductService _productService;
        private IOrderService _orderService;
        public AdminController(IUserService userService, IUserTypeService userTypeService, IDecentralizationService decentralizationService, IProductService productService, IOrderService orderService)
        {
            _userService = userService;
            _userTypeService = userTypeService;
            _decentralizationService = decentralizationService;
            _productService = productService;
            _orderService = orderService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.SumAccessTimes = HttpContext.Application["SumAccessTimes"].ToString();
                ViewBag.RealAccessTimes = HttpContext.Application["RealAccessTimes"].ToString();
                User user = Session["User"] as User;
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
        public ActionResult Login(User user)
        {
            //Check login
            User userCheck = _userService.CheckLogin(user.Email, user.Password);
            if (userCheck != null)
            {

                IEnumerable<Decentralization> decentralizations = _decentralizationService.GetDecentralizationByUserTypeID(userCheck.UserTypeID);
                string role = "";
                foreach (var item in decentralizations)
                {
                    role += item.Role.Name + ",";
                }

                role = role.Substring(0, role.Length - 1);
                Decentralization(userCheck.ID, role);

                Session["User"] = userCheck;
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
        public ActionResult InfoUser()
        {
            User user = Session["User"] as User;
            return View(user);
        }
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            User user = Session["User"] as User;
            //Check null
            if (user != null)
            {
                //Return view
                return Json(new
                {
                    ID = user.ID,
                    FullName = user.FullName,
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Image = user.Avatar,
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
        public ActionResult Edit(User user, HttpPostedFileBase ImageUpload)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.EmloyeeTypeIDEdit = new SelectList(_userTypeService.GetListUserType().OrderBy(x => x.Name), "ID", "Name");

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
                user.Avatar = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update emloyeetype
            User u = _userService.GetByID(user.ID);
            u.FullName = user.FullName;
            u.Address = user.Address;
            u.Email = user.Email;
            u.Avatar = user.Avatar;
            _userService.Update(u);
            Session["User"] = u;
            string Url = Request.Url.ToString();
            return RedirectToAction("InfoUser");
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string CurrentPassword, string NewPassword)
        {
            User user = Session["User"] as User;
            User userCheck = _userService.CheckLogin(user.Email, CurrentPassword);
            if (userCheck != null)
            {
                _userService.ResetPassword(user.ID, NewPassword);
                TempData["ResetPassword"] = "Success";
                return RedirectToAction("InfoUser");
            }
            else
            {
                ViewBag.Message = "Mật khẩu hiện tại không đúng!";
            }
            return View();
        }
    }
}