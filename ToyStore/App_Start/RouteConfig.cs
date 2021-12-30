using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ToyStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "SignUp",
               url: "dang-ky",
               defaults: new { controller = "Home", action = "SignUp"}
           );
            routes.MapRoute(
               name: "SignIn",
               url: "dang-nhap",
               defaults: new { controller = "Home", action = "SignIn" }
           );
            routes.MapRoute(
               name: "Cart",
               url: "gio-hang",
               defaults: new { controller = "Cart", action = "Checkout" }
           );
            routes.MapRoute(
               name: "Voucher",
               url: "quay-trung-thuong",
               defaults: new { controller = "Spin", action = "Index" }
           );
            routes.MapRoute(
               name: "ProductCategoryParent",
               url: "danh-muc-goc/{seo-keyword}-{id}",
               defaults: new { controller = "Product", action = "ProductCategoryParent", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "ProductCategory",
               url: "danh-muc/{seo-keyword}-{id}",
               defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Gender",
               url: "gioi-tinh/{seo-keyword}-{id}",
               defaults: new { controller = "Product", action = "Gender", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Age",
               url: "do-tuoi/{seo-keyword}-{id}",
               defaults: new { controller = "Product", action = "Ages", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Producer",
               url: "thuong-hieu/{seo-keyword}-{id}",
               defaults: new { controller = "Product", action = "Producer", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "ProductDetail",
               url: "san-pham/{seo-keyword}-{id}",
               defaults: new { controller = "Product", action = "Details", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Product",
               url: "san-pham-moi",
               defaults: new { controller = "Product", action = "NewProduct"}
           );
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
