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
    public class MemberController : Controller
    {
        IMemberService _memberService;
        ICustomerService _customerService;
        IOrderService _orderService;
        IOrderDetailService _orderDetailService;
        IProductService _productService;
        IRatingService _ratingService;
        IMemberDiscountCodeService _memberDiscountCodeService;
        public MemberController(IMemberService memberService, IOrderService orderService, IOrderDetailService orderDetailService, ICustomerService customerService, IProductService productService, IRatingService ratingService, IMemberDiscountCodeService memberDiscountCodeService)
        {
            _memberService = memberService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _customerService = customerService;
            _productService = productService;
            _ratingService = ratingService;
            _memberDiscountCodeService = memberDiscountCodeService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            Member member = Session["Member"] as Member;
            return View(member);
        }
        [HttpGet]
        public ActionResult EditName(int id)
        {
            var member = _memberService.GetByID(id);
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
            Member member = _memberService.GetByID(ID);
            member.FullName = FullName;
            _memberService.UpdateMember(member);
            Session["Member"] = member;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditAddress(int id)
        {
            var member = _memberService.GetByID(id);
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
            Member member = _memberService.GetByID(ID);
            member.Address = Address;
            _memberService.UpdateMember(member);
            Session["Member"] = member;
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
                Member member = Session["Member"] as Member;
                Member memberupdate = _memberService.GetByID(member.ID);
                memberupdate.Avatar = Avatar.FileName;
                _memberService.UpdateMember(memberupdate);
                Session["Member"] = memberupdate;
            }
            return RedirectToAction("Index");
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
            SentMail("Mã xác minh tài khoản ToyStore", email, "lapankhuongnguyen@gmail.com", "khuongpro2000fx18g399!@#<>?googlelapankhuongnguyen", "<p>Mã xác minh của bạn: " + captcha + "<br/>Hoặc xác minh nhanh bằng cách click vào link: " + urlBase + "/Member/ConfirmEmailLink/" + ID + "?Capcha=" + captcha + "</p>");
            return View();
        }
        [HttpGet]
        public ActionResult ConfirmEmailLink(int ID, string capcha)
        {
            Member member = _memberService.CheckCapcha(ID, capcha);
            if (member != null)
            {
                ViewBag.Message = "EmailConfirmed";
                //Gift
                _memberDiscountCodeService.GiftForNewMember(member.ID);
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
                //Gift
                _memberDiscountCodeService.GiftForNewMember(member.ID);
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
        public ActionResult CheckoutOrder(int ID, int page = 1)
        {
            string Name = _memberService.GetByID(ID).FullName;
            Customer customer = _customerService.GetAll().FirstOrDefault(x => x.FullName.Contains(Name));
            if (customer != null)
            {
                var orders = _orderService.GetByCustomerID(customer.ID);
                Member member = Session["Member"] as Member;
                ViewBag.ProductRating = _ratingService.GetListAllRating().Where(x => x.MemberID == member.ID);
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
            _orderService.Received(OrderID);

            Member member = Session["Member"] as Member;
            //Update AmountPurchased for member
            if (member != null)
            {
                _memberService.UpdateAmountPurchased(member.ID, _orderService.GetByID(OrderID).Total.Value);
            }
            return RedirectToAction("OrderDetail", new { ID = OrderID });
        }
    }
}