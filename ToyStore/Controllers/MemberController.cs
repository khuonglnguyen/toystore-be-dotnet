using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class MemberController : Controller
    {
        private IMemberService _memberService;
        private ICustomerService _customerService;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;
        private IProductService _productService;
        public MemberController(IMemberService memberService, IOrderService orderService, IOrderDetailService orderDetailService, ICustomerService customerService, IProductService productService)
        {
            _memberService = memberService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _customerService = customerService;
            _productService = productService;
        }
        [HttpGet]
        public ActionResult ConfirmEmail(int ID)
        {
            if (_memberService.GetByID(ID).EmailConfirmed)
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
            string email = _memberService.GetByID(ID).Email;
            ViewBag.Email = "Mã xác minh đã được gửi đến: " + email;
            _memberService.UpdateCapcha(ID, captcha);
            SentMail("Mã xác minh tài khoản ToyStore", email, "lapankhuongnguyen@gmail.com", "Garena009", "<p>Mã xác minh của bạn: " + captcha + "<br/>Hoặc xác minh nhanh bằng cách click vào link: " + urlBase + "/Member/ConfirmEmailLink/" + ID + "?Capcha=" + captcha + "</p>");
            return View();
        }
        [HttpGet]
        public ActionResult ConfirmEmailLink(int ID, string capcha)
        {
            Member member = _memberService.CheckCapcha(ID, capcha);
            if (member != null)
            {
                ViewBag.Message = "EmailConfirmed";
                return View();
            }
            ViewBag.Message = "Mã xác minh tài khoản không đúng";
            ViewBag.ID = ID;
            return View(new { ID = ID });
        }
        [HttpPost]
        public ActionResult ConfirmEmail(int ID, string capcha)
        {
            Member member = _memberService.CheckCapcha(ID, capcha);
            if (member != null)
            {
                ViewBag.Message = "EmailConfirmed";
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
        public ActionResult CheckoutOrder(int ID)
        {
            string Name = _memberService.GetByID(ID).FullName;
            Customer customer = _customerService.GetAll().FirstOrDefault(x => x.FullName.Contains(Name));
            if (customer != null)
            {
                IEnumerable<Order> orders = _orderService.GetByCustomerID(customer.ID);
                return View(orders);
            }
            return View();
        }
        [HttpGet]
        public ActionResult OrderDetail(int ID)
        {
            if (Session["Member"] == null)
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
            if (order.IsReceived)
            {
                ViewBag.Received = "Received";
            }
            return View(orderDetails);
        }
    }
}