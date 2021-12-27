using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class UserController : Controller
    {
        IUserService _userService;
        ICustomerService _customerService;
        IOrderService _orderService;
        IOrderDetailService _orderDetailService;
        IProductService _productService;
        IRatingService _ratingService;
        IUserDiscountCodeService _userDiscountCodeService;
        ICartService _cartService;
        IUsersSpinService _usersSpinService;
        public UserController(IUserService userService, IOrderService orderService, IOrderDetailService orderDetailService, ICustomerService customerService, IProductService productService, IRatingService ratingService, IUserDiscountCodeService userDiscountCodeService, ICartService cartService, IUsersSpinService usersSpinService)
        {
            _userService = userService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _customerService = customerService;
            _productService = productService;
            _ratingService = ratingService;
            _userDiscountCodeService = userDiscountCodeService;
            _cartService = cartService;
            _usersSpinService = usersSpinService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            User user = Session["User"] as User;
            return View(user);
        }
        [HttpGet]
        public ActionResult EditName(int id)
        {
            var member = _userService.GetByID(id);
            //Check null
            if (member != null)
            {
                //Return view
                return Json(new
                {
                    ID = member.ID,
                    FullName = member.FullName,
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
        public ActionResult EditName(int ID, string FullName)
        {
            User user = _userService.GetByID(ID);
            user.FullName = FullName;
            _userService.Update(user);
            IEnumerable<Customer> customers = _customerService.GetAll();
            foreach (Customer item in customers)
            {
                if (item.PhoneNumber == user.PhoneNumber)
                {
                    item.FullName = user.FullName;
                    _customerService.Update(item);
                }
            }
            Session["User"] = user;
            TempData["EditName"] = "Sucess";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditAddress(int id)
        {
            var member = _userService.GetByID(id);
            //Check null
            if (member != null)
            {
                //Return view
                return Json(new
                {
                    ID = member.ID,
                    Address = member.Address,
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
        public ActionResult EditAddress(int ID, string Address)
        {
            User user = _userService.GetByID(ID);
            user.Address = Address;
            _userService.Update(user);
            Session["Member"] = user;
            TempData["EditAddress"] = "Sucess";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult EditAvatar(HttpPostedFileBase Avatar)
        {
            if (Avatar != null)
            {
                //Get file name
                var fileName = Path.GetFileName(Avatar.FileName);
                //Get path
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                //Check exitst
                if (!System.IO.File.Exists(path))
                {
                    //Add image into folder
                    Avatar.SaveAs(path);
                }
                User user = Session["User"] as User;
                User userUpdate = _userService.GetByID(user.ID);
                userUpdate.Avatar = Avatar.FileName;
                _userService.Update(userUpdate);
                Session["User"] = userUpdate;
                TempData["EditAvatar"] = "Sucess";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ConfirmEmail(int ID)
        {
            if (_userService.GetByID(ID).EmailConfirmed)
            {
                ViewBag.Message = "EmailConfirmed";
                return View();
            }
            string strString = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            int randomCharIndex = 0;
            char randomChar;
            string captcha = "";
            for (int i = 0; i < 10; i++)
            {
                randomCharIndex = random.Next(0, strString.Length);
                randomChar = strString[randomCharIndex];
                captcha += Convert.ToString(randomChar);
            }
            string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
            ViewBag.ID = ID;
            string email = _userService.GetByID(ID).Email;
            ViewBag.Email = "Mã xác minh đã được gửi đến: " + email;
            _userService.UpdateCapcha(ID, captcha);
            SentMail("Mã xác minh tài khoản ToyStore", email, "khuongip564gb@gmail.com", "googlekhuongip564gb", "<p>Mã xác minh của bạn: " + captcha + "<br/>Hoặc xác minh nhanh bằng cách click vào link: " + urlBase + "/User/ConfirmEmailLink/" + ID + "?Capcha=" + captcha + "</p>");
            return View();
        }
        [HttpGet]
        public ActionResult ConfirmEmailLink(int ID, string capcha)
        {
            User user = _userService.CheckCapcha(ID, capcha);
            if (user != null)
            {
                ViewBag.Message = "EmailConfirmed";
                //Gift
                _userDiscountCodeService.GiftForNewUser(user.ID);
                return View();
            }
            ViewBag.Message = "Mã xác minh tài khoản không đúng";
            ViewBag.ID = ID;
            return View(new { ID = ID });
        }
        [HttpPost]
        public ActionResult ConfirmEmail(int ID, string capcha)
        {
            User user = _userService.CheckCapcha(ID, capcha);
            if (user != null)
            {
                ViewBag.Message = "EmailConfirmed";
                //Gift
                _userDiscountCodeService.GiftForNewUser(user.ID);
                return View();
            }
            ViewBag.Message = "Mã xác minh tài khoản không đúng";
            ViewBag.ID = ID;
            return View(new { ID = ID });
        }
        public void SentMail(string Title, string ToEmail, string FromEmail, string Password, string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);
            mail.From = new MailAddress(ToEmail);
            mail.Subject = Title;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(FromEmail, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        public ActionResult Error(int CustomerID)
        {
            ViewBag.ID = CustomerID;
            return View();
        }
        [HttpGet]
        public ActionResult CheckoutOrder()
        {
            User user = Session["User"] as User;
            if (user != null)
            {
                string Phone = _userService.GetByID(user.ID).PhoneNumber;
                Customer customer = _customerService.GetAll().FirstOrDefault(x => x.PhoneNumber == Phone);
                if (customer != null)
                {
                    var orders = _orderService.GetAll().Where(x => x.Customer.PhoneNumber == customer.PhoneNumber && x.Customer.IsMember == true);
                    ViewBag.ProductRating = _ratingService.GetListAllRating().Where(x => x.UserID == user.ID);
                    return View(orders);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult OrderDetail(int ID)
        {
            if (Session["User"] == null)
            {
                return View();
            }
            if (ID == null)
            {
                return null;
            }
            Order order = _orderService.GetByID(ID);
            IEnumerable<OrderDetail> orderDetails = _orderDetailService.GetByOrderID(ID);
            if (orderDetails == null)
            {
                return null;
            }
            ViewBag.OrderID = ID;
            if (order.IsApproved)
            {
                ViewBag.Approved = "Approved";
            }
            if (order.IsDelivere)
            {
                ViewBag.Delivere = "Delivere";
            }
            if (order.IsReceived)
            {
                ViewBag.Received = "Received";
            }
            ViewBag.Total = order.Total;
            return View(orderDetails);
        }
        public ActionResult GetDataProduct(int ID)
        {
            Product product = _productService.GetByID(ID);
            return Json(new
            {
                ID = product.ID,
                Name = product.Name,
                Image = product.Image1,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Received(int OrderID)
        {
            User user = Session["User"] as User;
            //Update AmountPurchased for member
            if (user != null)
            {
                _orderService.Received(OrderID);
                _userService.UpdateAmountPurchased(user.ID, _orderService.GetByID(OrderID).Total.Value);
                //Add once spin
                _usersSpinService.AddOnceSpin(user.ID);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("OrderDetail", new { ID = OrderID });
        }
        public JsonResult Cancel(int ID)
        {
            Order order = _orderService.GetByID(ID);
            order.IsCancel = true;
            _orderService.Update(order);
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeleteAccount()
        {
            User user = Session["User"] as User;
            return View(user);
        }
        [HttpPost]
        public JsonResult DeleteAccount(string Password)
        {
            User user = Session["User"] as User;
            User userCheck = _userService.CheckLogin(user.Email, Password);
            if (userCheck != null)
            {
                _userService.Block(userCheck);
                _cartService.RemoveCartDeleteAccount(user.ID);
                TempData["DeleteAccount"] = "Success";
                Session["User"] = null;
                Session["Cart"] = null;
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["IncorretPassword"] = "true";
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
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
                _userService.ResetPassword(userCheck.ID, NewPassword);
                TempData["ResetPassword"] = "Success";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Mật khẩu hiện tại không đúng!";
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetOrderJson()
        {
            var list = _orderService.GetAll().Where(x => x.IsApproved == false).OrderByDescending(x => x.DateOrder).Take(5).Select(x => new { ID = x.ID, CustomerName = x.Customer.FullName, DateOrder = (DateTime.Now - x.DateOrder).Minutes });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}