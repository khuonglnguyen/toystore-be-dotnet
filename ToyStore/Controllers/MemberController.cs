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
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpGet]
        public ActionResult ConfirmEmail(int ID)
        {
            string strString = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            int randomCharIndex = 0;
            char randomChar;
            string captcha = "";
            for (int i = 0; i < 15; i++)
            {
                randomCharIndex = random.Next(0, strString.Length);
                randomChar = strString[randomCharIndex];
                captcha += Convert.ToString(randomChar);
            }
            ViewBag.ID = ID;
            _memberService.UpdateCapcha(ID, captcha);
            SentMail("Mã xác minh tài khoản ToyStore", "khuongip564gb@gmail.com", "lapankhuongnguyen@gmail.com", "Garena009", "Mã xác minh của bạn: " + captcha);
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmEmail(int ID, string capcha)
        {
            Member member = _memberService.CheckCapcha(ID, capcha);
            if (member != null)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Error", new { CustomerID = ID});
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
    }
}