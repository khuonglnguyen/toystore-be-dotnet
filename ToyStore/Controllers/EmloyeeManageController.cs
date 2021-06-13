using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToyStore.Models;
using ToyStore.Service;

namespace ToyStore.Controllers
{
    [Authorize(Roles = "EmloyeeManage")]
    public class EmloyeeManageController : Controller
    {
        IEmloyeeService _emloyeeService;
        IEmloyeeTypeService _emloyeeTypeService;
        public EmloyeeManageController(IEmloyeeService emloyeeService, IEmloyeeTypeService emloyeeTypeService)
        {
            _emloyeeService = emloyeeService;
            _emloyeeTypeService = emloyeeTypeService;
        }
        // GET: Emloyee
        [HttpGet]
        public ActionResult Index(int page = 1, string keyword = "")
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            Emloyee emloyee = Session["Emloyee"] as Emloyee;
            //Get data for DropdownList
            ViewBag.EmloyeeTypeID = new SelectList(_emloyeeTypeService.GetListEmloyeeType().OrderBy(x => x.Name), "ID", "Name");

            ViewBag.EmloyeeTypeIDEdit = ViewBag.EmloyeeTypeID;

            ViewBag.EmloyeeTypeIDDetail = ViewBag.EmloyeeTypeID;

            int pageSize = 10;
            //Get emloyee list
            if (keyword != "")
            {
                var emloyees = _emloyeeService.GetList(emloyee.ID).Where(x => x.FullName.Contains(keyword));
                ViewBag.Emloyees = emloyees;
                PagedList<Emloyee> listEmloyee = new PagedList<Emloyee>(emloyees, page, pageSize);
                ViewBag.KeyWord = keyword;
                //Check null
                if (listEmloyee != null)
                {
                    ViewBag.Page = page;
                    //Return view
                    return View(listEmloyee);
                }
                else
                {
                    //return 404
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                var emloyees = _emloyeeService.GetList(emloyee.ID);
                ViewBag.Emloyees = emloyees;
                PagedList<Emloyee> listEmloyee = new PagedList<Emloyee>(emloyees, page, pageSize);
                ViewBag.KeyWord = keyword;
                //Check null
                if (listEmloyee != null)
                {
                    ViewBag.Page = page;
                    //Return view
                    return View(listEmloyee);
                }
                else
                {
                    //return 404
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
        }
        [HttpPost]
        public JsonResult ListName(string Prefix)
        {
            List<string> names = _emloyeeService.GetEmloyeeListName(Prefix).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Create(Emloyee emloyee, HttpPostedFileBase ImageUpload, int page)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            } 
            int errorCount = 0;
            //Check content image
            if (ImageUpload != null && ImageUpload.ContentLength > 0)
            {
                //Check format iamge
                if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                {
                    //Set viewbag
                    ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                    //increase by 1 unit errorCount
                    errorCount++;
                }
                else
                {
                    //Get file name
                    var fileName = Path.GetFileName(ImageUpload.FileName);
                    //Get path
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //Check exitst
                    if (!System.IO.File.Exists(path))
                    {
                        //Add image into folder
                        ImageUpload.SaveAs(path);
                    }
                }
            }
            //Set new value image for emloyee
            emloyee.Image = ImageUpload.FileName;
            //Set TempData for checking in view to show swal
            TempData["create"] = "Success";
            //Create emloyee
            _emloyeeService.Add(emloyee);
            //Return view
            return RedirectToAction("Index", new { page = 1 });
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get emloyee
            var emloyee = _emloyeeService.GetByID(id);

            //Get data for DropdownList
            ViewBag.EmloyeeTypeIDEdit = new SelectList(_emloyeeTypeService.GetListEmloyeeType().OrderBy(x => x.Name), "ID", "Name", emloyee.EmloyeeTypeID);

            //Check null
            if (emloyee != null)
            {
                //Return view
                return Json(new
                {
                    ID = emloyee.ID,
                    FullName = emloyee.FullName,
                    Address = emloyee.Address,
                    Email = emloyee.Email,
                    PhoneNumber = emloyee.PhoneNumber,
                    Image = emloyee.Image,
                    EmloyeeTypeID = emloyee.EmloyeeTypeID,
                    IsActive = emloyee.IsActive,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Edit(Emloyee emloyee, HttpPostedFileBase ImageUpload, int page, int EmloyeeTypeIDEdit)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.EmloyeeTypeIDEdit = new SelectList(_emloyeeTypeService.GetListEmloyeeType().OrderBy(x => x.Name), "ID", "Name");

            if (ImageUpload != null)
            {
                int errorCount = 0;
                //Check content image
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    //Check format iamge
                    if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/gif")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            ImageUpload.SaveAs(path);
                        }
                    }
                }
                //Set new value image for emloyee
                emloyee.Image = ImageUpload.FileName;
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update emloyeetype
            Emloyee e = _emloyeeService.GetByID(emloyee.ID);
            e.FullName = emloyee.FullName;
            e.Address = emloyee.Address;
            e.Email = emloyee.Email;
            e.Image = emloyee.Image;
            e.EmloyeeTypeID = EmloyeeTypeIDEdit;
            _emloyeeService.Update(e);
            string Url = Request.Url.ToString();
            return RedirectToAction("Index", new { page = page });
        }
        public void Block(int id)
        {
            //Get emloyee by ID
            var emloyee = _emloyeeService.GetByID(id);
            _emloyeeService.Block(emloyee);
        }
        public void Active(int id)
        {
            //Get emloyee by ID
            var emloyee = _emloyeeService.GetByID(id);
            _emloyeeService.Active(emloyee);
        }
    }
}