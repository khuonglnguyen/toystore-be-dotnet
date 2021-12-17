using PagedList;
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
    [Authorize(Roles = "OrderManage")]
    public class OrderManageController : Controller
    {
        private IOrderService _orderService;
        private ICustomerService _customerService;
        private IOrderDetailService _orderDetailService;
        private IProductService _productService;
        public OrderManageController(IOrderService orderService, ICustomerService customerService, IOrderDetailService orderDetailService,IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _orderDetailService = orderDetailService;
            _productService = productService;
        }
        // GET: OrderManage
        public JsonResult GetOrderJson()
        {
            var list = _orderService.GetAll().Where(x=>x.IsApproved == false).OrderByDescending(x => x.DateOrder).Take(5).Select(x => new { ID = x.ID, CustomerName = x.Customer.FullName, DateOrder = (DateTime.Now - x.DateOrder).Minutes});
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotApproval(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<Order> orderList = _orderService.GetOrderNotApproval();
            PagedList<Order> orderListPaging = new PagedList<Order>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }
        public ActionResult NotDelivery(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<Order> orderList = _orderService.GetOrderNotDelivery();
            PagedList<Order> orderListPaging = new PagedList<Order>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }

        public ActionResult DeliveredAndPaid(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<Order> orderList = _orderService.GetOrderDeliveredAndPaid();
            PagedList<Order> orderListPaging = new PagedList<Order>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }
        public ActionResult ApprovedAndNotDelivery(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<Order> orderList = _orderService.ApprovedAndNotDelivery();
            PagedList<Order> orderListPaging = new PagedList<Order>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }
        public ActionResult DeliveredList(int page = 1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<Order> orderList = _orderService.GetDelivered();
            PagedList<Order> orderListPaging = new PagedList<Order>(orderList, page, 10);

            IEnumerable<Customer> customerList = _customerService.GetAll();
            ViewBag.CustomerList = customerList;
            return View(orderListPaging);
        }
        [HttpGet]
        public ActionResult OrderApproval(int ID)
        {
            Order order = _orderService.Approved(ID);
            //Get email customer
            string Email = _customerService.GetEmailByID(order.CustomerID);
            SentMail("Đơn hàng của bạn đã được duyệt", Email, "khuongip564gb@gmail.com", "googlekhuongip564gb", "Vào đơn hàng của bạn để xem thông tin chi tiết");
            return RedirectToAction("ApprovedAndNotDelivery");
        }
        [HttpGet]
        public ActionResult Delivered(int ID)
        {
            Order order = _orderService.Delivered(ID);
            //Get email customer
            string Email = _customerService.GetEmailByID(order.CustomerID);
            string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
            SentMail("Đơn hàng của bạn đã được giao cho đối tác vận chuyển", Email, "khuongip564gb@gmail.com", "googlekhuongip564gb", "Vào đơn hàng của bạn để xem thông tin chi tiết. Sau khi nhận được đơn hàng, bạn vui lòng click vào link sau để xác nhận đã nhận được đơn hàng từ đơn vị vận chuyển: " + urlBase + "/OrderManage/Received/" + ID+"");
            return RedirectToAction("DeliveredList");
        }
        [HttpGet]
        public ActionResult Detail(int ID)
        {
            if (ID == null)
            {
                return null;
            }
            IEnumerable<OrderDetail> orderDetails = _orderDetailService.GetByOrderID(ID);
            if (orderDetails == null)
            {
                return null;
            }
            ViewBag.OrderID = ID;
            Order order = _orderService.GetByID(ID);
            ViewBag.IsApproved = order.IsApproved;
            return View(orderDetails);
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
        [HttpGet]
        public ActionResult Received(int ID)
        {
            Order order = _orderService.GetByID(ID);
            order.IsReceived = true;
            order.IsPaid = true;
            _orderService.Update(order);
            IEnumerable<OrderDetail> orderDetail = _orderDetailService.GetByOrderID(order.ID);
            foreach (var item in orderDetail)
            {
                _productService.UpdateQuantity(item.ProductID, item.Quantity);
                _productService.UpdatePurchasedCount(item.ProductID, item.Quantity);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}