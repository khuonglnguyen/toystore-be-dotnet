using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    public class DecentralizationManageController : Controller
    {
        private IEmloyeeTypeService _emloyeeTypeService;
        private IRoleService _roleService;
        private IDecentralizationService _decentralizationService;
        public DecentralizationManageController(IEmloyeeTypeService emloyeeTypeService,IRoleService roleService,IDecentralizationService decentralizationService)
        {
            _emloyeeTypeService = emloyeeTypeService;
            _roleService = roleService;
            _decentralizationService = decentralizationService;
        }
        // GET: DecentralizationManage
        public ActionResult Index(int page=1)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<EmloyeeType> emloyeeTypes = _emloyeeTypeService.GetListEmloyeeType();
            PagedList<EmloyeeType> emloyeeTypesList = new PagedList<EmloyeeType>(emloyeeTypes, page, 10);
            return View(emloyeeTypesList);
        }
        [HttpGet]
        public ActionResult Decentralization(int id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            EmloyeeType emloyeeType = _emloyeeTypeService.GetEmloyeeTypeByID(id);
            if (emloyeeType == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleList = _roleService.GetRoleList();
            ViewBag.ListDecentralization = _decentralizationService.GetDecentralizationByEmloyeeTypeID(id);
            return View(emloyeeType);
        }
        [HttpPost]
        public ActionResult Decentralization(int EmloyeeTypeID, IEnumerable<Decentralization> decentralizations)
        {
            //Trường hợp: Nếu đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            //Bước 1: Xóa những quyền cũ thuộc loại tv đó
            var ListDecentralization = _decentralizationService.GetDecentralizationByEmloyeeTypeID(EmloyeeTypeID);
            if (ListDecentralization.Count() != 0)
            {
                _decentralizationService.RemoveRange(ListDecentralization);
            }
            if (decentralizations != null)
            {
                //Kiểm tra danh sách quyền được check
                foreach (var item in decentralizations)
                {
                    item.EmloyeeTypeID = EmloyeeTypeID;
                    //Nếu được check thì insert dữ liệu vào bảng phân quyền
                    _decentralizationService.Add(item);
                }
            }
            return RedirectToAction("Index");
        }
    }
}