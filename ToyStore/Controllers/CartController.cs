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
    public class CartController : Controller
    {
        private IProductService _productService;
        private ICustomerService _customerService;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;
        private ICartService _cartService;
        private IDiscountCodeService _discountCodeService;
        private IDiscountCodeDetailService _discountCodeDetailService;
        public CartController(IProductService productService, ICustomerService customerService, IOrderService orderService, IOrderDetailService orderDetailService, ICartService cartService, IDiscountCodeService discountCodeService, IDiscountCodeDetailService discountCodeDetailService)
        {
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _cartService = cartService;
            _discountCodeService = discountCodeService;
            _discountCodeDetailService = discountCodeDetailService;
        }
        // GET: Cart
        public List<ItemCart> GetCart()
        {
            Member member = Session["Member"] as Member;
            if (member != null)
            {
                if (_cartService.CheckCartMember(member.ID))
                {
                    List<ItemCart> listCart = _cartService.GetCart(member.ID);
                    foreach (ItemCart item in listCart)
                    {
                        if (item.Image == null || item.Image == "")
                        {
                            item.Image = _productService.GetByID(item.ProductID).Image1;
                        }
                        if (item.Price == 0)
                        {
                            item.Price = _productService.GetByID(item.ProductID).PromotionPrice;
                        }
                    }
                    Session["Cart"] = listCart;
                    return listCart;
                }
            }
            else
            {
                List<ItemCart> listCart = Session["Cart"] as List<ItemCart>;
                //Check null session Cart
                if (listCart == null)
                {
                    //Initialization listCart
                    listCart = new List<ItemCart>();
                    Session["Cart"] = listCart;
                    return listCart;
                }
                return listCart;
            }
            return null;
        }
        [HttpPost]
        public ActionResult AddItemCart(int ID)
        {
            //Check product already exists in DB
            Product product = _productService.GetByID(ID);
            if (product == null)
            {
                //product does not exist
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<ItemCart> listCart = GetCart();
            //If member
            Member member = Session["Member"] as Member;
            if (member != null)
            {
                //Case 1: If product already exists in Member Cart
                if (_cartService.CheckProductInCart(ID, member.ID))
                {
                    _cartService.AddQuantityProductCartMember(ID, member.ID);
                }
                else
                {
                    //Case 2: If product does not exist in Member Cart
                    //Get product
                    ItemCart itemCart = new ItemCart(ID);
                    itemCart.MemberID = member.ID;
                    _cartService.AddCartIntoMember(itemCart);
                }
                List<ItemCart> carts = _cartService.GetCart(member.ID);
                foreach (ItemCart item in carts)
                {
                    if (item.Image == null || item.Image == "")
                    {
                        item.Image = _productService.GetByID(item.ProductID).Image1;
                    }
                }
                Session["Cart"] = carts;
                ViewBag.TotalQuanity = GetTotalQuanity();
                ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
                return PartialView("CartPartial");
            }
            else
            {
                if (listCart != null)
                {
                    //Case 1: If product already exists in session Cart
                    ItemCart itemCart = listCart.SingleOrDefault(n => n.ProductID == ID);
                    if (itemCart != null)
                    {
                        //Check inventory before letting customers make a purchase
                        if (product.Quantity < itemCart.Quantity)
                        {
                            return View("ThongBao");
                        }
                        itemCart.Quantity++;
                        itemCart.Total = itemCart.Quantity * product.PromotionPrice;
                        ViewBag.TotalQuanity = GetTotalQuanity();
                        ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
                        return PartialView("CartPartial");
                    }
                    //Case 2: If product does not exist in the Session Cart
                    ItemCart item = new ItemCart(ID);
                    item.Image = _productService.GetByID(item.ProductID).Image1;
                    listCart.Add(item);
                }
            }
            ViewBag.TotalQuanity = GetTotalQuanity();
            ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
            return PartialView("CartPartial");
        }
        [HttpPost]
        public ActionResult CheckQuantityAdd(int ID)
        {
            //Check product already exists in DB
            Product product = _productService.GetByID(ID);
            if (product == null)
            {
                //product does not exist
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<ItemCart> listCart = GetCart();
            //Check quantity
            if (listCart != null)
            {
                int sum = 0;
                foreach (ItemCart item in listCart.Where(x => x.ProductID == ID))
                {
                    sum += item.Quantity;
                }
                if (product.Quantity > sum)
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            else
            {
                if (product.Quantity > 0)
                {

                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
        }
        [HttpPost]
        public ActionResult CheckQuantityUpdate(int ID, int Quantity)
        {
            //Check product already exists in DB
            Product product = _productService.GetByID(ID);
            if (product.Quantity >= Quantity)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public ActionResult CartPartial()
        {
            if (GetTotalQuanity() == 0)
            {
                ViewBag.TotalQuanity = 0;
                ViewBag.TotalPrice = 0;
                return PartialView();
            }
            ViewBag.TotalQuanity = GetTotalQuanity();
            ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
            return PartialView();
        }
        public double GetTotalQuanity()
        {
            List<ItemCart> listCart = Session["Cart"] as List<ItemCart>;
            if (listCart == null)
            {
                return 0;
            }
            return listCart.Sum(n => n.Quantity);
        }
        public decimal GetTotalPrice()
        {
            //Lấy giỏ hàng
            List<ItemCart> listCart = Session["Cart"] as List<ItemCart>;
            if (listCart == null)
            {
                return 0;
            }
            return listCart.Sum(n => n.Total);
        }
        public ActionResult Checkout()
        {
            ViewBag.TotalQuantity = GetTotalQuanity();
            Member member = Session["Member"] as Member;
            try
            {
                Session["Cart"] = GetCart();
                ViewBag.DiscountCodeDetailListByMemer = _discountCodeDetailService.GetDiscountCodeDetailListByMember(member.ID);
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult EditCart(int ID)
        {
            //Check null session cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Check whether the product exists in the database or not?
            Product product = _productService.GetByID(ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<ItemCart> listCart = GetCart();
            //Check if the product exists in the shopping cart
            ItemCart item = listCart.SingleOrDefault(n => n.ProductID == ID);
            if (item == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Cart = listCart;
            return Json(new
            {
                ID = item.ID,
                Price = item.Price,
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Image = item.Image,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditCart(int ID, int Quantity)
        {
            //Check stock quantity
            Product product = _productService.GetByID(ID);
            //Updated quantity in cart Session
            List<ItemCart> listCart = GetCart();
            //Get products from within listCart to update
            ItemCart itemCartUpdate = listCart.Find(n => n.ProductID == ID);
            itemCartUpdate.Quantity = Quantity;
            itemCartUpdate.Total = itemCartUpdate.Quantity * itemCartUpdate.Price;

            Member member = Session["Member"] as Member;
            if (member != null)
            {
                //Update Cart Quantity Member
                _cartService.UpdateQuantityCartMember(Quantity, ID, member.ID);
                Session["Cart"] = listCart;
            }

            return RedirectToAction("Checkout");
        }
        [HttpGet]
        public ActionResult RemoveItemCart(int ID)
        {
            //Check null session Cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Check whether the product exists in the database or not?
            Product product = _productService.GetByID(ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<ItemCart> listCart = GetCart();
            //Check if the product exists in the shopping cart
            ItemCart item = listCart.SingleOrDefault(n => n.ProductID == ID);
            if (item == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Remove item cart
            listCart.Remove(item);
            Member member = Session["Member"] as Member;
            if (member != null)
            {
                _cartService.RemoveCart(ID, member.ID);
                List<ItemCart> carts = _cartService.GetCart(member.ID);
                Session["Cart"] = carts;
            }
            ViewBag.TotalQuantity = GetTotalQuanity();
            return PartialView("CheckoutPartial");
        }
        [HttpPost]
        public ActionResult AddOrder(Customer customer, int NumberDiscountPass = 0, string CodePass = "")
        {
            //Check null session cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Customer customercheck = new Customer();
            bool status = false;
            //Is Customer
            Customer customerNew = new Customer();
            if (Session["Member"] == null)
            {
                //Insert customer into DB
                customerNew = customer;
                customerNew.IsMember = false;
                _customerService.AddCustomer(customerNew);
            }
            else
            {
                //Is member
                Member member = Session["Member"] as Member;
                customercheck = _customerService.GetAll().FirstOrDefault(x => x.FullName.Contains(member.FullName));
                if (customercheck != null)
                {
                    status = true;
                }
                else
                {
                    customerNew.FullName = member.FullName;
                    customerNew.Address = member.Address;
                    customerNew.Email = member.Email;
                    customerNew.PhoneNumber = member.PhoneNumber;
                    customerNew.IsMember = true;
                    _customerService.AddCustomer(customerNew);
                }
            }
            //Add order
            Order order = new Order();
            if (status)
            {
                order.CustomerID = customercheck.ID;
            }
            else
            {
                order.CustomerID = customerNew.ID;
            }
            order.DateOrder = DateTime.Now;
            order.DateShip = DateTime.Now.AddDays(3);
            order.IsPaid = false;
            order.IsDelete = false;
            order.IsDelivere = false;
            order.IsApproved = false;
            order.IsReceived = false;
            order.IsCancel = false;
            order.Offer = NumberDiscountPass;
            _orderService.AddOrder(order);
            //Add order detail
            List<ItemCart> listCart = GetCart();
            decimal sumtotal = 0;
            foreach (ItemCart item in listCart)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderID = order.ID;
                orderDetail.ProductID = item.ProductID;
                orderDetail.Quantity = item.Quantity;
                orderDetail.Price = item.Product.PromotionPrice;
                _orderDetailService.AddOrderDetail(orderDetail);
                sumtotal += orderDetail.Quantity * orderDetail.Price;
                if (Session["member"] != null)
                {
                    //Remove Cart
                    _cartService.RemoveCart(item.ProductID, item.MemberID);
                }
            }
            if (NumberDiscountPass != 0)
            {
                _orderService.UpdateTotal(order.ID, sumtotal - (sumtotal / 100 * NumberDiscountPass));
            }
            else
            {
                _orderService.UpdateTotal(order.ID, sumtotal);
            }
            if (CodePass != "")
            {
                //Set discountcode used
                _discountCodeDetailService.Used(CodePass);
            }
            Session["Code"] = null;
            Session["num"] = null;
            Session["Cart"] = null;
            if (status)
            {
                SentMail("Đặt hàng thành công", customercheck.Email, "lapankhuongnguyen@gmail.com", "khuongpro2000fx18g399!@#<>?googlelapankhuongnguyen", "<p style=\"font-size:20px\">Cảm ơn bạn đã đặt hàng<br/>Mã đơn hàng của bạn là: " + order.ID + "</p>");
            }
            else
            {
                SentMail("Đặt hàng thành công", customerNew.Email, "lapankhuongnguyen@gmail.com", "khuongpro2000fx18g399!@#<>?googlelapankhuongnguyen", "<p style=\"font-size:20px\">Cảm ơn bạn đã đặt hàng<br/>Mã đơn hàng của bạn là: " + order.ID + "</p>");
            }
            return RedirectToAction("Message");
        }
        [HttpGet]
        public ActionResult Message()
        {
            return View();
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
        [HttpPost]
        public ActionResult Choose(string Code, string CodeInput)
        {
            if (CodeInput != "")
            {
                int numcheck = _discountCodeDetailService.GetDiscountByCode(CodeInput);
                if (numcheck != null)
                {
                    Session["Code"] = CodeInput;
                    Session["num"] = numcheck;
                }
                else
                {
                    int num = _discountCodeDetailService.GetDiscountByCode(Code);
                    Session["Code"] = Code;
                    Session["num"] = num;
                }
            }
            else
            {
                int num = _discountCodeDetailService.GetDiscountByCode(Code);
                Session["Code"] = Code;
                Session["num"] = num;
            }
            return RedirectToAction("Checkout");
        }
        public ActionResult CancelDiscount()
        {
            Session["Code"] = null;
            Session["num"] = null;
            return RedirectToAction("Checkout");
        }
    }
}