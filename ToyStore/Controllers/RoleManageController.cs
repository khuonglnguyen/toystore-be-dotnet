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
    public class RoleManageController : Controller
    {
        private IRoleService _roleService;
        public RoleManageController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        // GET: RoleManage
        public ActionResult List(int page = 1)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            IEnumerable<Role> Roles = _roleService.GetRoleList();
            PagedList<Role> RolesProduct = new PagedList<Role>(Roles, page, 12);
            return View(RolesProduct);
        }
        [HttpPost]
        public ActionResult AddRole(Role role)
        {
            _roleService.AddRole(role);
            return RedirectToAction("List");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return null;
            }
            //Get role
            var role = _roleService.GetRoleByID(id);
            if (role == null)
            {
                return null;
            }
            return Json(new
            {
                ID = role.ID,
                Name = role.Name,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost] 
        public ActionResult Edit(Role role)
        {
            _roleService.UpdateRole(role);
            return RedirectToAction("List");
        }
        [HttpGet]
        public ActionResult Block(int id, int row)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Check id null
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get role by ID
            var role = _roleService.GetRoleByID(id);
            //Check null
            if (role == null)
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _roleService.BlockRole(role);
            ViewBag.Row = row;
            return PartialView("RoleActivePartial", _roleService.GetRoleByID(id));
        }
        [HttpGet]
        public ActionResult Active(int id, int row)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Check id null
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get role by ID
            var role = _roleService.GetRoleByID(id);
            //Check null
            if (role == null)
            {
                //return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _roleService.ActiveRole(role);
            ViewBag.Row = row;
            return PartialView("RoleActivePartial", _roleService.GetRoleByID(id));
        }
        public ActionResult RoleActivePartial(int ID)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            return PartialView("RoleActivePartial", _roleService.GetRoleByID(ID));
        }
    }
}