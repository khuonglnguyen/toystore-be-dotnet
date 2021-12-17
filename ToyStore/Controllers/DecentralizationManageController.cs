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
    [Authorize(Roles = "DecentralizationManage")]
    public class DecentralizationManageController : Controller
    {
        private IUserTypeService _userTypeService;
        private IRoleService _roleService;
        private IDecentralizationService _decentralizationService;
        public DecentralizationManageController(IUserTypeService userTypeService,IRoleService roleService,IDecentralizationService decentralizationService)
        {
            _userTypeService = userTypeService;
            _roleService = roleService;
            _decentralizationService = decentralizationService;
        }
        // GET: DecentralizationManage
        public ActionResult Index(int page=1)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<UserType> UserTypes = _userTypeService.GetListUserType();
            PagedList<UserType> UserTypesList = new PagedList<UserType>(UserTypes, page, 10);
            return View(UserTypesList);
        }
        [HttpGet]
        public ActionResult Decentralization(int id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UserType UserType = _userTypeService.GetUserTypeByID(id);
            if (UserType == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleList = _roleService.GetRoleList();
            ViewBag.ListDecentralization = _decentralizationService.GetDecentralizationByUserTypeID(id);
            return View(UserType);
        }
        [HttpPost]
        public ActionResult Decentralization(int UserTypeID, IEnumerable<Decentralization> decentralizations)
        {
            //Trường hợp: Nếu đã tiến hành phân quyền rồi nhưng muốn phân quyền lại
            //Bước 1: Xóa những quyền cũ thuộc loại user đó
            var ListDecentralization = _decentralizationService.GetDecentralizationByUserTypeID(UserTypeID);
            if (ListDecentralization.Count() != 0)
            {
                _decentralizationService.RemoveRange(ListDecentralization);
            }
            if (decentralizations != null)
            {
                //Kiểm tra danh sách quyền được check
                foreach (var item in decentralizations)
                {
                    item.UserTypeID = UserTypeID;
                    //Nếu được check thì insert dữ liệu vào bảng phân quyền
                    _decentralizationService.Add(item);
                }
            }
            return RedirectToAction("Index");
        }
    }
}