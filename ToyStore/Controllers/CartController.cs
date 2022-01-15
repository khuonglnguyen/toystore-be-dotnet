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
        private IUserService _userService;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;
        private ICartService _cartService;
        private IDiscountCodeService _discountCodeService;
        private IDiscountCodeDetailService _discountCodeDetailService;
        public CartController(IProductService productService, IUserService userService, IOrderService orderService, IOrderDetailService orderDetailService, ICartService cartService, IDiscountCodeService discountCodeService, IDiscountCodeDetailService discountCodeDetailService)
        {
            _productService = productService;
            _userService = userService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _cartService = cartService;
            _discountCodeService = discountCodeService;
            _discountCodeDetailService = discountCodeDetailService;
        }
        // GET: Cart
        public List<ItemCart> GetCart()
        {
            User user = Session["User"] as User;
            if (user != null)
            {
                if (_cartService.CheckCartUser(user.ID))
                {
                    List<ItemCart> listCart = _cartService.GetCart(user.ID);
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
            //If User
            User user = Session["User"] as User;
            if (user != null)
            {
                //Case 1: If product already exists in Member Cart
                if (_cartService.CheckProductInCart(ID, user.ID))
                {
                    _cartService.AddQuantityProductCartUser(ID, user.ID);
                }
                else
                {
                    //Case 2: If product does not exist in User Cart
                    //Get product
                    ItemCart itemCart = new ItemCart(ID);
                    itemCart.UserID = user.ID;
                    _cartService.AddCartIntoUser(itemCart);
                }
                List<ItemCart> carts = _cartService.GetCart(user.ID);
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
                        itemCart.Total = itemCart.Quantity * product.Price;
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
            var f = listCart.Sum(n => n.Total);
            return f;
        }
        public ActionResult Checkout()
        {
            ViewBag.TotalQuantity = GetTotalQuanity();
            User user = Session["User"] as User;
            try
            {
                Session["Cart"] = GetCart();
                ViewBag.DiscountCodeDetailListByUser = _discountCodeDetailService.GetDiscountCodeDetailListByUser(user.ID);
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

            User user = Session["User"] as User;
            if (user != null)
            {
                //Update Cart Quantity Member
                _cartService.UpdateQuantityCartUser(Quantity, ID, user.ID);
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
            User user = Session["User"] as User;
            if (user != null)
            {
                _cartService.RemoveCart(ID, user.ID);
                List<ItemCart> carts = _cartService.GetCart(user.ID);
                Session["Cart"] = carts;
            }
            ViewBag.TotalQuantity = GetTotalQuanity();
            return PartialView("CheckoutPartial");
        }
        [HttpPost]
        public ActionResult AddOrder(User user, int NumberDiscountPass = 0, string CodePass = "", string payment = "")
        {
            //Check null session cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            User usercheck = new User();
            bool status = true;
            //Is User
            User userOrder = new User();
            if (Session["User"] == null)
            {
                status = false;
                //Check Email & Phone
                if (_userService.CheckEmailPhone(user.Email, user.PhoneNumber))
                {
                    //Insert user into DB
                    userOrder = user;
                    userOrder.Avatar = "user.png";
                    userOrder.EmailConfirmed = false;
                    _userService.Add(userOrder);
                }
                else
                {
                    //Update user in DB
                    userOrder = _userService.GetByPhoneNumber(user.PhoneNumber);
                    userOrder.Address = user.Address;
                    userOrder.FullName = user.FullName;
                    _userService.Update(userOrder);
                }
            }
            //Add order
            Models.Order order = new Models.Order();
            if (status)
            {
                order.UserID = (Session["User"] as User).ID;
            }
            else
            {
                order.UserID = userOrder.ID;
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
            Models.Order o = _orderService.AddOrder(order);
            Session["OrderId"] = o.ID;
            //Add order detail
            List<ItemCart> listCart = GetCart();
            decimal sumtotal = 0;
            foreach (ItemCart item in listCart)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderID = order.ID;
                orderDetail.ProductID = item.ProductID;
                orderDetail.Quantity = item.Quantity;
                orderDetail.Price = item.Price;
                _orderDetailService.AddOrderDetail(orderDetail);
                sumtotal += orderDetail.Quantity * orderDetail.Price;
                if (Session["User"] != null)
                {
                    //Remove Cart
                    _cartService.RemoveCart(item.ProductID, item.UserID);
                }
            }
            _orderService.UpdateTotal(order.ID, sumtotal);

            if (CodePass != "")
            {
                //Set discountcode used
                _discountCodeDetailService.Used(CodePass);
            }

            // Payment
            if (payment == "paypal")
            {
                return RedirectToAction("PaymentWithPaypal", "Payment");
            }
            else if (payment == "momo")
            {
                return RedirectToAction("PaymentWithMomo", "Payment");
            }


            SentMail("Đặt hàng thành công", user.Email, "khuongip564gb@gmail.com", "google..khuongip564gb", "<p style=\"font-size:20px\">Cảm ơn bạn đã đặt hàng<br/>Mã đơn hàng của bạn là: " + order.ID);



            Session.Remove("Code");
            Session.Remove("num");
            Session.Remove("Cart");
            Session.Remove("OrderId");

            return RedirectToAction("Message", new { mess = "Đặt hàng thành công" });
        }

        [HttpGet]
        public ActionResult Message(string mess)
        {
            ViewBag.Message = mess;
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
                int numcheck = _discountCodeDetailService.GetDiscountByCodeInput(CodeInput);
                if (numcheck != -1)
                {
                    Session["Code"] = CodeInput;
                    Session["num"] = numcheck;
                }
                else
                {
                    int num = _discountCodeDetailService.GetDiscountByCodeInput(Code);
                    if (num == -1)
                    {
                        TempData["Message"] = "Mã giảm giá không đúng";
                        return RedirectToAction("Checkout");
                    }
                    Session["Code"] = Code;
                    Session["num"] = num;
                }
            }
            else
            {
                int num = _discountCodeDetailService.GetDiscountByCode(Code);
                if (num == -1)
                {
                    TempData["Message"] = "Mã giảm giá không đúng";
                    return RedirectToAction("Checkout");
                }
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