using ClosedXML.Excel;
using Microsoft.Reporting.WebForms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    [Authorize(Roles = "StatisticManage")]
    public class StatisticManageController : Controller
    {
        private IOrderService _orderService;
        private IProductService _productService;
        private IUserService _userService;
        private ISupplierService _supplierService;
        private IAccessTimesCountService _accessTimesCountService;
        public StatisticManageController(IOrderService orderService, IProductService productService, IUserService userService, ISupplierService supplierService, IAccessTimesCountService accessTimesCountService)
        {
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            _supplierService = supplierService;
            _accessTimesCountService = accessTimesCountService;
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
        public void DownloadExcelStatisticStocking()
        {
            User user = Session["User"] as User;

            IEnumerable<Product> products = _productService.GetProductListStocking();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.FullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Mã SP";
            ws.Cells["B6"].Value = "Tên SP";
            ws.Cells["C6"].Value = "Số lượng tồn";

            int rowStart = 7;
            foreach (var item in products)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Name;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Quantity;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Danh sách tồn kho.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [HttpGet]
        public ActionResult StatisticUser()
        {
            IEnumerable<User> users = _userService.GetUserListForStatistic();
            return View(users);
        }
        [HttpGet]
        public ActionResult StatisticSupplier()
        {
            IEnumerable<Supplier> suppliers = _supplierService.GetSupplierList();
            return View(suppliers);
        }
        [HttpGet]
        public void DownloadExcelStatisticSupplier()
        {
            User user = Session["User"] as User;

            IEnumerable<Supplier> suppliers = _supplierService.GetSupplierList();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.FullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Mã Nhà Cung Cấp";
            ws.Cells["B6"].Value = "Tên Nhà Cung Cấp";
            ws.Cells["C6"].Value = "Địa Chỉ";
            ws.Cells["D6"].Value = "Email";
            ws.Cells["E6"].Value = "Số Điện Thoại";
            ws.Cells["F6"].Value = "Tình Trạng";
            ws.Cells["G6"].Value = "Doanh Số";

            int rowStart = 7;
            foreach (var item in suppliers)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Name;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.Address;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Email;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Phone;
                if (item.IsActive)
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Đang hợp tác";
                else
                {
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Đã ngừng hợp tác";
                }
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.TotalAmount;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Nhà cung cấp tốt nhất.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [Authorize(Roles = "StatisticProductSold")]
        [HttpGet]
        public ActionResult StatisticProductSold(DateTime from, DateTime to)
        {
            IEnumerable<Product> products = _productService.GetProductListSold(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(products);
        }
        [HttpGet]
        public void DownloadExcelStatisticProductSold(DateTime from, DateTime to)
        {
            User user = Session["User"] as User;

            IEnumerable<Product> products = _productService.GetProductListSold(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.FullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Mã SP";
            ws.Cells["B6"].Value = "Tên SP";
            ws.Cells["C6"].Value = "Só Lượng Đã Bán";

            int rowStart = 7;
            foreach (var item in products)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Name;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.PurchasedCount;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Sản phẩm đã bán.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [HttpGet]
        public ActionResult StatisticOrder(DateTime from, DateTime to)
        {
            IEnumerable<Order> orders = _orderService.GetListOrderStatistic(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(orders);
        }
        [HttpGet]
        public void DownloadExcelStatisticOrder(DateTime from, DateTime to)
        {
            User user = Session["User"] as User;

            IEnumerable<Order> orders = _orderService.GetListOrderStatistic(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.FullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Mã Hóa Đơn";
            ws.Cells["B6"].Value = "Tên KH";
            ws.Cells["C6"].Value = "Ngày Đặt";
            ws.Cells["D6"].Value = "Ngày Giao";
            ws.Cells["E6"].Value = "Ưu Đãi";
            ws.Cells["F6"].Value = "Tình Trạng";
            ws.Cells["G6"].Value = "Thành Tiền";

            int rowStart = 7;
            foreach (var item in orders)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.User.FullName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.DateOrder.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.DateShip.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("E{0}", rowStart)].Value = "-" + item.Offer + "%";
                if (item.IsReceived)
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Hoàn thành";
                else
                {
                    ws.Cells[string.Format("F{0}", rowStart)].Value = "Chưa hoàn thành";
                }
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Total;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Đơn đặt hàng.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [HttpGet]
        public ActionResult StatisticAccessTime(DateTime from, DateTime to)
        {
            IEnumerable<AccessTimesCount> accessTimesCounts = _accessTimesCountService.GetListAccessTimeCountStatistic(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(accessTimesCounts);
        }
        [HttpGet]
        public void DownloadExcelStatisticStatisticAccessTime(DateTime from, DateTime to)
        {
            User user = Session["User"] as User;

            IEnumerable<AccessTimesCount> accessTimesCounts = _accessTimesCountService.GetListAccessTimeCountStatistic(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.FullName;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Ngày";
            ws.Cells["B6"].Value = "Số Lượng Truy Cập";

            int rowStart = 7;
            foreach (var item in accessTimesCounts)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Date.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.AccessTimes;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Số lượng truy cập.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
    }
}