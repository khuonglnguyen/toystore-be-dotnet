using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ToyStore.Config;
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
        public ActionResult AddOrder(Customer customer, int NumberDiscountPass = 0, string CodePass = "", string payment = "")
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
            Models.Order order = new Models.Order();
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

            // Payment
            if (payment != "")
            {
                return RedirectToAction("PaymentWithPaypal");
            }

            Session["Code"] = null;
            Session["num"] = null;
            Session["Cart"] = null;
            Session["OrderId"] = null;

            return RedirectToAction("Message", new { mess = "Đặt hàng thành công" });
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("Message", new { mess = "Lỗi" });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Message", new
                {
                    mess = ex.Message
                });
            }
            Session["Code"] = null;
            Session["num"] = null;
            Session["Cart"] = null;
            //update paid
            if (_orderService.Paid(Convert.ToInt32(Session["OrderId"])))
            {
                //on successful payment, show success page to user.  
                return RedirectToAction("Message", new { mess = "Đặt hàng và thanh toán thành công" });
            }
            return RedirectToAction("Message", new { mess = "Lỗi" });
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apicontext, string redirectURl)
        {
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            if (Session["Cart"] != null)
            {
                var d = GetCurrencyExchange("VND", "USD");
                List<ItemCart> cart = (List<ItemCart>)Session["Cart"];
                foreach (var item in cart)
                {
                    decimal p = Math.Round(item.Price * d, 0);
                    itemList.items.Add(new Item()
                    {
                        name = item.Name,
                        currency = "USD",
                        price = p.ToString(),
                        quantity = item.Quantity.ToString(),
                        sku = "sku"
                    });
                }

                var payer = new Payer()
                {
                    payment_method = "paypal"
                };

                var redirUrl = new RedirectUrls()
                {
                    cancel_url = redirectURl + "&Cancel=true",
                    return_url = redirectURl
                };

                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = itemList.items.Sum(x => int.Parse(x.price) * int.Parse(x.quantity)).ToString()
                };

                var amount = new Amount()
                {
                    currency = "USD",
                    total = details.subtotal,
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Transaction Description",
                    invoice_number = Convert.ToString((new Random()).Next(100000)),
                    amount = amount,
                    item_list = itemList
                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };
            }

            return this.payment.Create(apicontext);
        }

        public Decimal GetCurrencyExchange(String localCurrency, String foreignCurrency)
        {
            var code = $"{localCurrency}_{foreignCurrency}";
            var newRate = FetchSerializedData(code);
            return newRate;
        }

        private Decimal FetchSerializedData(String code)
        {
            var url = "https://free.currconv.com/api/v7/convert?q=" + code + "&compact=y&apiKey=7cf3529b5d3edf9fa798";
            var webClient = new WebClient();
            var jsonData = String.Empty;

            var conversionRate = 1.0m;
            try
            {
                jsonData = webClient.DownloadString(url);
                var jsonObject = new JavaScriptSerializer().Deserialize<Dictionary<string, Dictionary<string, decimal>>>(jsonData);
                var result = jsonObject[code];
                conversionRate = result["val"];

            }
            catch (Exception) { }

            return conversionRate;
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
                int numcheck = _discountCodeDetailService.GetDiscountByCode(CodeInput);
                if (numcheck != -1)
                {
                    Session["Code"] = CodeInput;
                    Session["num"] = numcheck;
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