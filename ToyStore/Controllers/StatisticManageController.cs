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
        private IMemberService _memberService;
        private ISupplierService _supplierService;
        private IAccessTimesCountService _accessTimesCountService;
        public StatisticManageController(IOrderService orderService, IProductService productService, IMemberService memberService, ISupplierService supplierService, IAccessTimesCountService accessTimesCountService)
        {
            _orderService = orderService;
            _productService = productService;
            _memberService = memberService;
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
        [HttpGet]
        public ActionResult StatisticMember()
        {
            IEnumerable<Member> members = _memberService.GetMemberListForStatistic();
            return View(members);
        }
        [HttpGet]
        public ActionResult DownloadExcelStatisticMember()
        {
            IEnumerable<Member> members = _memberService.GetMemberListForStatistic();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Mã Thành viên"),
                                            new DataColumn("Tên thành viên"),
                                            new DataColumn("Địa chỉ"),
                                            new DataColumn("Email"),
                                            new DataColumn("Số điện thoại"),
                                            new DataColumn("Loại thành viên"),
                                            new DataColumn("Doanh số")
                                            });
            foreach (var member in members)
            {
                dt.Rows.Add(member.ID, member.FullName, member.Address, member.Email, member.PhoneNumber, member.MemberType.Name, "đ" + member.AmountPurchased.Value.ToString("#,##"));
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Khách hàng thành viên tiềm năng.xlsx");
                }
            }
        }
        [HttpGet]
        public ActionResult StatisticSupplier()
        {
            IEnumerable<Supplier> suppliers = _supplierService.GetSupplierList();
            return View(suppliers);
        }
        [HttpGet]
        public ActionResult DownloadExcelStatisticSupplier()
        {
            IEnumerable<Supplier> suppliers = _supplierService.GetSupplierList();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Mã nhà cung cấp"),
                                            new DataColumn("Tên nhà cung cấp"),
                                            new DataColumn("Địa chỉ"),
                                            new DataColumn("Email"),
                                            new DataColumn("Số điện thoại"),
                                            new DataColumn("Tình trạng"),
                                            new DataColumn("Doanh số")
                                            });
            foreach (var supplier in suppliers)
            {
                string status = "";
                if (supplier.IsActive)
                {
                    status = "Đang hợp tác";
                }
                else
                {
                    status = "Ngừng hợp tác";
                }
                dt.Rows.Add(supplier.ID, supplier.Name, supplier.Address, supplier.Email, supplier.Phone, status, "đ" + supplier.TotalAmount.Value.ToString("#,##"));
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Những nhà cung cấp tốt nhất.xlsx");
                }
            }
        }
        [HttpGet]
        public ActionResult StatisticProductSold(DateTime from, DateTime to)
        {
            IEnumerable<Product> products = _productService.GetProductListSold(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(products);
        }
        [HttpGet]
        public ActionResult DownloadExcelStatisticProductSold(DateTime from, DateTime to)
        {
            IEnumerable<Product> products = _productService.GetProductListSold(from, to);
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Mã SP"),
                                            new DataColumn("Tên SP"),
                                            new DataColumn("Số lượng đã bán")
                                            });
            foreach (var product in products)
            {
                dt.Rows.Add(product.ID, product.Name, product.PurchasedCount);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Những sản phẩm bán chạy.xlsx");
                }
            }
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
        public ActionResult DownloadExcelStatisticOrder(DateTime from, DateTime to)
        {
            IEnumerable<Order> orders = _orderService.GetListOrderStatistic(from, to);
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Mã hóa đơn"),
                                            new DataColumn("Tên khách hàng"),
                                            new DataColumn("Ngày đặt"),
                                            new DataColumn("Ngày giao"),
                                            new DataColumn("Ưu đãi"),
                                            new DataColumn("Tình trạng"),
                                            new DataColumn("Thành tiển")
                                            });
            foreach (var order in orders)
            {
                string status = "";
                if (order.IsReceived)
                {
                    status = "Hoàn thành";
                }
                else
                {
                    status = "Chưa hoàn thành";
                }
                dt.Rows.Add(order.ID, order.Customer.FullName, order.DateOrder, order.DateShip, order.Offer, status, "đ" + order.Total.Value.ToString("#,##"));
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Đơn đặt hàng hoàn thành.xlsx");
                }
            }
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
        public ActionResult DownloadExcelStatisticStatisticAccessTime(DateTime from, DateTime to)
        {
            IEnumerable<AccessTimesCount> accessTimesCounts = _accessTimesCountService.GetListAccessTimeCountStatistic(from, to);
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Ngày"),
                                            new DataColumn("Số lượt truy cập")
                                            });
            foreach (var access in accessTimesCounts)
            {
                dt.Rows.Add(access.Date.ToShortDateString(), access.AccessTimes);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Thống kê lượt truy cập.xlsx");
                }
            }
        }
    }
}