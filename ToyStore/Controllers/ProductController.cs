using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;
using System.Data.Entity;

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
        private IQAService _qAService;
        private IUserService _userService;
        private IProductViewedService _productViewedService;
        private IRatingService _ratingService;
        private IOrderDetailService _orderDetailService;

        ToyStore2021Entities context = new ToyStore2021Entities();

        public ProductController(IProductService productService, IProducerService producerService, ISupplierService supplierService, IProductCategoryService productCategoryService, IAgeService ageService, IProductCategoryParentService productCategoryParentService, IGenderService genderService, IUserService userService, IQAService qAService, IProductViewedService productViewedService, IRatingService ratingService, IOrderDetailService orderDetailService)
        {
            _productService = productService;
            _producerService = producerService;
            _supplierService = supplierService;
            _productCategoryService = productCategoryService;
            _ageService = ageService;
            _productCategoryParentService = productCategoryParentService;
            _genderService = genderService;
            _qAService = qAService;
            _userService = userService;
            _productViewedService = productViewedService;
            _ratingService = ratingService;
            _orderDetailService = orderDetailService;
        }
        #endregion
        public ActionResult Search(string keyword, int page = 1)
        {
            var listProduct = _productService.GetProductList(keyword);
            ViewBag.ListProduct = listProduct;
            if (keyword == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Keyword = keyword;
            var products = _productService.GetProductList(keyword);
            PagedList<Product> listProductSearch = new PagedList<Product>(products, page, 12);
            return View(listProductSearch);
        }
        public ActionResult Details(int ID)
        {
            var product = _productService.GetByID(ID);
            var listProduct = _productService.GetProductListByCategory(product.CategoryID).Where(x => x.ID != ID);
            ViewBag.ListProduct = listProduct;
            IEnumerable<QA> listQA = _qAService.GetQAByProductID(ID).OrderByDescending(x => x.DateQuestion);
            ViewBag.CommentQA = listQA;
            IEnumerable<User> listUser = _userService.GetList();
            ViewBag.UserList = listUser;

            if (Session["User"] != null)
            {
                User user = Session["User"] as User;
                _productViewedService.AddProductViewByUser(ID, user.ID);
            }
            //Add view count
            if (Request.Cookies["ViewedPage"] != null)
            {
                if (Request.Cookies["ViewedPage"][string.Format("ID_{0}", ID)] == null)
                {
                    HttpCookie cookie = (HttpCookie)Request.Cookies["ViewedPage"];
                    cookie[string.Format("ID_{0}", ID)] = "1";
                    cookie.Expires = DateTime.Now.AddMinutes(1);
                    Response.Cookies.Add(cookie);

                    _productService.AddViewCount(ID);
                }
            }
            else
            {
                HttpCookie cookie = new HttpCookie("ViewedPage");
                cookie[string.Format("ID_{0}", ID)] = "1";
                cookie.Expires = DateTime.Now.AddMinutes(1);
                Response.Cookies.Add(cookie);

                _productService.AddViewCount(ID);
            }
            //Get rating
            ViewBag.Rating = _ratingService.GetRating(ID);
            //Get list rating
            ViewBag.ListRating = _ratingService.GetListRating(ID);
            return View(product);
        }
        [HttpGet]
        public ActionResult Ages(int ID, string keyword = "", int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.ageID = ID;
            Age ages = _ageService.GetAgeByID(ID);
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
            IEnumerable<Product> products = _productService.GetProductListLast();
            listProductPaging = new PagedList<Product>(products, page, 3);
            return View(listProductPaging);
        }
        public ActionResult ProductViewed(int page = 1)
        {
            var listProduct = _productService.GetProductList().OrderByDescending(x => x.ViewCount).Take(5);
            ViewBag.ListProduct = listProduct;

            User user = Session["User"] as User;
            PagedList<Product> listProductPaging;
            IEnumerable<Product> products = _productService.GetProductListViewedByUserID(user.ID);
            listProductPaging = new PagedList<Product>(products, page, 10);
            return View(listProductPaging);
        }
        public ActionResult DeleteHistoryView()
        {
            User user = Session["User"] as User;
            _productViewedService.Delete(user.ID);
            TempData["DeleteHistoryView"] = "Sucess";
            return RedirectToAction("ProductViewed");
        }
        public ActionResult ProductPartial(Product product)
        {
            //Get rating
            ViewBag.Rating = _ratingService.GetRating(product.ID);
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
        public ActionResult AddQuestion(int productID, int userID, string Question)
        {
            QA qa = new QA();
            qa.UserAskID = userID;
            qa.Question = Question;
            qa.ProductID = productID;
            qa.DateQuestion = DateTime.Now;
            qa.DateAnswer = DateTime.Now;
            qa.UserAnswerID = 2;
            _qAService.AddQA(qa);
            return RedirectToAction("QAPartial", new { ID = productID });
        }
        public ActionResult CommentPartial(int ID)
        {
            IEnumerable<User> listUser = _userService.GetList();
            ViewBag.UserList = listUser;
            return PartialView("_CommentPartial");
        }
        public ActionResult QAPartial(int ID)
        {
            IEnumerable<QA> listQA = context.QAs.Include(x => x.User).Where(x => x.ProductID == ID).OrderByDescending(x => x.DateQuestion).ToList();
            ViewBag.QAList = listQA;
            return PartialView("_QAPartial");
        }
        [HttpPost]
        public ActionResult Rating(Rating rating, int OrderDetailID)
        {
            User user = Session["User"] as User;
            rating.UserID = user.ID;
            _ratingService.AddRating(rating);
            _orderDetailService.SetIsRating(OrderDetailID);
            return RedirectToAction("Details", "Product", new { ID = rating.ProductID });
        }
        [HttpGet]
        public ActionResult GetDataQuesion(int id)
        {
            if (id == null)
            {
                return null;
            }
            //Get product catetgory
            var qAs = _qAService.GetQAByID(id);
            if (qAs == null)
            {
                return null;
            }
            return Json(new
            {
                ID = qAs.ID,
                UserAskID = qAs.UserAskID,
                ProductID = qAs.ProductID,
                Question = qAs.Question,
                Answer = qAs.Answer,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeleteQA(int ID, int ProductID)
        {
            _qAService.Delete(ID);
            IEnumerable<QA> listQA = context.QAs.Include(x => x.User).Where(x => x.ProductID == ID).OrderByDescending(x => x.DateQuestion).ToList();
            ViewBag.QAList = listQA;
            return RedirectToAction("QAPartial", new { ID = ProductID });
        }
        [HttpPost]
        public ActionResult EditQuestion(QA qA)
        {
            QA qa = _qAService.GetQAByID(qA.ID);
            qa.Question = qA.Question;
            _qAService.UpdateQA(qa);
            return RedirectToAction("Details", "Product", new { ID = qA.ProductID });
        }
    }
}