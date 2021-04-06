using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class ProductController : Controller
    {
        #region Initialize
        private IProductService _productService;
        private IProducerService _producerService;
        private ISupplierService _supplierService;
        private IProductCategoryService _productCategoryService;
        private IAgeService _ageService;
        private IProductCategoryParentService _productCategoryParentService;
        private IGenderService _genderService;
        private ICommentService _commentService;
        private IMemberService _memberService;
        private IQAService _qaService;
        private IEmloyeeService _emloyeeService;
        private IProductViewedService _productViewedService;
        private IRatingService _ratingService;
        private IOrderDetailService _orderDetailService;

        public ProductController(IProductService productService, IProducerService producerService, ISupplierService supplierService, IProductCategoryService productCategoryService, IAgeService ageService, IProductCategoryParentService productCategoryParentService, IGenderService genderService, ICommentService commentService, IMemberService memberService, IQAService qAService, IEmloyeeService emloyeeService, IProductViewedService productViewedService, IRatingService ratingService, IOrderDetailService orderDetailService)
        {
            _productService = productService;
            _producerService = producerService;
            _supplierService = supplierService;
            _productCategoryService = productCategoryService;
            _ageService = ageService;
            _productCategoryParentService = productCategoryParentService;
            _genderService = genderService;
            _commentService = commentService;
            _memberService = memberService;
            _qaService = qAService;
            _emloyeeService = emloyeeService;
            _productViewedService = productViewedService;
            _ratingService = ratingService;
            _orderDetailService = orderDetailService;
        }
        #endregion
        public ActionResult Search(string keyword, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;
            if (keyword == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Keyword = keyword;
            //Get proudct category list with keyword
            var products = _productService.GetProductList(keyword);
            PagedList<Product> listProductSearch = new PagedList<Product>(products, page, 12);
            //Check null
            if (listProduct != null)
            {
                ViewBag.message = "Hiển thị kết quả tìm kiếm với '" + keyword + "";
                //Return view
                return View(listProductSearch);
            }
            else
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // GET: Product
        public ActionResult Details(int ID)
        {
            var product = _productService.GetByID(ID);
            var producer = _producerService.GetByID(product.ProducerID);
            var supplier = _supplierService.GetByID(product.SupplierID);
            var listProduct = _productService.GetProductListByCategory(product.CategoryID);
            var listAge = _ageService.GetAgeList();
            var listEmloyee = _emloyeeService.GetList();
            ViewBag.ProducerName = producer.Name;
            ViewBag.ListEmloyee = listEmloyee;
            ViewBag.SupplierName = supplier.Name;
            ViewBag.ListProduct = listProduct;
            ViewBag.Age = listAge.Single(x => x.ID == product.AgeID).Name;

            IEnumerable<Comment> listComment = _commentService.GetCommentByProductID(ID).OrderByDescending(x => x.Date);
            ViewBag.CommentList = listComment;
            IEnumerable<QA> listQA = _qaService.GetQAByProductID(ID).OrderByDescending(x => x.DateQuestion);
            ViewBag.CommentQA = listQA;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;

            if (Session["Member"] != null)
            {
                Member member = Session["Member"] as Member;
                _productViewedService.AddProductViewByMember(ID, member.ID);
            }
            //Add view count
            if (Request.Cookies["ViewedPage"] != null)
            {
                if (Request.Cookies["ViewedPage"][string.Format("ID_{0}", ID)] == null)
                {
                    HttpCookie cookie = (HttpCookie)Request.Cookies["ViewedPage"];
                    cookie[string.Format("ID_{0}", ID)] = "1";
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    _productService.AddViewCount(ID);
                }
            }
            else
            {
                HttpCookie cookie = new HttpCookie("ViewedPage");
                cookie[string.Format("ID_{0}", ID)] = "1";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);

                _productService.AddViewCount(ID);
            }
            //Get rating
            ViewBag.Rating = _ratingService.GetRating(ID);
            return View(product);
        }
        [HttpGet]
        public ActionResult Ages(int ID, string keyword = "", int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.ageID = ID;
            Ages ages = _ageService.GetAgeByID(ID);
            ViewBag.Name = "Độ tuổi " + ages.Name;

            PagedList<Product> listProductPaging;
            if (keyword != "")
            {
                IEnumerable<Product> products = _productService.GetProductListByAge(ID).Where(x => x.Name.Contains(keyword));
                listProductPaging = new PagedList<Product>(products, page, 12);
            }
            else
            {
                IEnumerable<Product> products = _productService.GetProductListByAge(ID);
                listProductPaging = new PagedList<Product>(products, page, 12);
            }
            return View(listProductPaging);
        }
        public ActionResult Producer(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.producerID = ID;
            Producer producer = _producerService.GetByID(ID);
            ViewBag.Name = "Thương hiệu " + producer.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByProducer(ID);
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult ProductCategory(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.productCategoryID = ID;
            ProductCategory productCategory = _productCategoryService.GetByID(ID);
            ViewBag.Name = "Danh mục " + productCategory.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByCategory(ID);
            listProductPaging = new PagedList<Product>(products, page, 9);
            return View(listProductPaging);
        }
        public ActionResult ProductCategoryParent(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.productCategoryParentID = ID;
            ProductCategoryParent productCategoryParent = _productCategoryParentService.GetByID(ID);
            ViewBag.Name = "Danh mục gốc " + productCategoryParent.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListByCategoryParent(ID);
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult Gender(int ID, int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.GenderID = ID;
            Gender gender = _genderService.GetGenderByID(ID);
            ViewBag.Name = "Giới tính " + gender.Name;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products;
            if (ID != 3)
            {
                products = _productService.GetProductListByGender(ID);
            }
            else
            {
                products = _productService.GetProductList();
            }
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult NewProduct(int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListIsNew();
            listProductPaging = new PagedList<Product>(products, page, 12);
            return View(listProductPaging);
        }
        public ActionResult ProductViewed(int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            Member member = Session["Member"] as Member;
            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListViewedByMemberID(member.ID);
            listProductPaging = new PagedList<Product>(products, page, 10);
            return View(listProductPaging);
        }
        public ActionResult DeleteHistoryView()
        {
            Member member = Session["Member"] as Member;
            _productViewedService.Delete(member.ID);
            return RedirectToAction("ProductViewed");
        }
        public ActionResult ProductPartial(Product product)
        {
            return PartialView(product);
        }
        public PartialViewResult FilterProductList(string type, int ID, int min = 0, int max = 0, int discount = 0, int page = 1)
        {
            PagedList<Product> listProductPaging = null;
            if (type == "Ages")
            {
                ViewBag.Name = "Độ tuổi " + _ageService.GetAgeByID(ID).Name;
                IEnumerable<Product> products = _productService.GetProductFilterByAges(ID, min, max, discount);
                listProductPaging = new PagedList<Product>(products, page, 2);
            }

            ViewBag.Min = min;
            ViewBag.Max = max;
            ViewBag.Type = type;
            ViewBag.ID = ID;
            //Check null
            if (listProductPaging != null)
            {
                //Return view
                return PartialView("ProductContainerPartial", listProductPaging);
            }
            else
            {
                //return 404
                return null;
            }
        }

        [HttpGet]
        public ActionResult AddComment(int productID, int memberID, string content)
        {
            Comment comment = new Comment();
            comment.MemberID = memberID;
            comment.Content = content;
            comment.ProductID = productID;
            comment.Date = DateTime.Now;
            _commentService.AddComment(comment);

            IEnumerable<Comment> listComment = _commentService.GetCommentByProductID(productID).OrderByDescending(x => x.Date);
            ViewBag.CommentList = listComment;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;
            return PartialView("_CommentPartial");
        }
        [HttpGet]
        public ActionResult AddQuestion(int productID, int memberID, string Question)
        {
            QA qa = new QA();
            qa.MemberID = memberID;
            qa.Question = Question;
            qa.ProductID = productID;
            qa.DateQuestion = DateTime.Now;
            qa.DateAnswer = DateTime.Now;
            qa.EmloyeeID = 1;
            _qaService.AddQA(qa);

            IEnumerable<QA> listQA = _qaService.GetQAByProductID(productID).OrderByDescending(x => x.DateQuestion);
            ViewBag.QAList = listQA;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;
            return PartialView("_QAPartial");
        }
        public ActionResult CommentPartial(int ID)
        {
            IEnumerable<Comment> listComment = _commentService.GetCommentByProductID(ID).OrderByDescending(x => x.Date);
            ViewBag.CommentList = listComment;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;
            return PartialView("_CommentPartial");
        }
        public ActionResult QAPartial(int ID)
        {
            IEnumerable<QA> listQA = _qaService.GetQAByProductID(ID).OrderByDescending(x => x.DateQuestion);
            ViewBag.QAList = listQA;
            IEnumerable<Member> listMember = _memberService.GetMemberList();
            ViewBag.MemberList = listMember;
            return PartialView("_QAPartial");
        }
        [HttpPost]
        public ActionResult Rating(Rating rating, int OrderDetailID)
        {
            Member member = Session["Member"] as Member;
            rating.MemberID = member.ID;
            _ratingService.AddRating(rating);
            _orderDetailService.SetIsRating(OrderDetailID);
            string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
            return Redirect(urlBase);
        }
    }
}