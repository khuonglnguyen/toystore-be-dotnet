using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class StatisticManageController : Controller
    {
        private IOrderService _orderService;
        private IProductService _productService;
        public StatisticManageController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }
        // GET: Statistic
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StatisticStocking()
        {
            IEnumerable<Product> products = _productService.GetProductListStocking();
            return View(products);
        }
        [HttpGet]
        public ActionResult DownloadExcelStatisticStocking()
        {
            IEnumerable<Product> products = _productService.GetProductListStocking();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Mã SP"),
                                            new DataColumn("Tên SP"),
                                            new DataColumn("Số lượng tồn")
                                            });

            foreach (var product in products)
            {
                dt.Rows.Add(product.ID, product.Name, product.Quantity);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sản phẩm tồn kho.xlsx");
                }
            }
        }
    }
}