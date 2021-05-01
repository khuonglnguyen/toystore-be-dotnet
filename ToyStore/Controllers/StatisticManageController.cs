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
        public StatisticManageController(IOrderService orderService, IProductService productService, IMemberService memberService)
        {
            _orderService = orderService;
            _productService = productService;
            _memberService = memberService;
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
    }
}