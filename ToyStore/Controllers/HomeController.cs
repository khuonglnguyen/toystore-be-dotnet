using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Data;
using ToyStore.Data.Repository;
using ToyStore.Models;

namespace ToyStore.Controllers
{
    public class HomeController : Controller
    {
        IProductCategory _productCategory;
        public HomeController()
        {
            this._productCategory = new ProductCategoryRepository(new ToyStoreDbContext());
        }
        public ActionResult Index(int id)
        {
            //ViewBag.ProducerID = new SelectList(_product.Pro.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ProductCategory productCategory = _productCategory.GetByID(id);
            if (productCategory != null)
            {
                return View(productCategory);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}