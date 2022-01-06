using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Service;
using ToyStore.Models;
using System.Net;

namespace ToyStore.Controllers
{
    [Authorize(Roles = "Chat")]
    public class ChatController : Controller
    {
        private IMessageService _messageService;
        private IUserService _userService;
        public ChatController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }
        public ActionResult Index()
        {
            User user = Session["User"] as User;
            List<Message> listMessage = _messageService.GetIndexAdmin(user.ID);
            return View(listMessage);
        }

        public ActionResult Chating(int WithUserID, int MessageID = 0)
        {
            IEnumerable<Message> listMessage;
            if (MessageID != 0)
            {
                //Update Sent
                if (_messageService.UpdateSent(MessageID))
                {
                    listMessage = _messageService.GetAllByUserID(WithUserID);
                    ViewBag.UserFullName = _userService.GetByID(WithUserID).FullName;
                    return View(listMessage);
                }
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            User user = _userService.GetByID(WithUserID);
            ViewBag.UserFullName = user.FullName;
            return View();
        }

        // GET: Message
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetAllMessageChating(int UserID)
        {
            var listMessage = _messageService.GetAllByUserID(UserID).Select(x =>
            new
            {
                ID = x.ID,
                FromUserID = x.FromUserID,
                Content = x.Content,
                CreatedDate = x.CreatedDate.Value,
                FromUserName = x.User.FullName,
                FromAvatarUser = x.User.Avatar
            });
            return Json(listMessage, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetLastMessageClient(int UserID)
        {
            var message = _messageService.GetLastByUserID(UserID);
            return Json(new
            {
                FromUserID = message.FromUserID,
                Content = message.Content,
                CreatedDate = message.CreatedDate.Value,
                FromUserName = message.User.FullName,
                FromAvatarUser = message.User.Avatar
            }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Send(int FromUserID, int ToUserID, string Content, string Side)
        {
            if (Side == "Client")
            {
                Message message = new Message();
                _messageService.UpdateSentClient();
                message.Sent = false;
                message.FromUserID = FromUserID;
                message.ToUserID = ToUserID;
                message.Content = Content;
                message.CreatedDate = DateTime.Now;

                if (_messageService.Add(message))
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Message message = new Message();

                message.FromUserID = FromUserID;
                message.ToUserID = ToUserID;
                message.Content = Content;
                message.CreatedDate = DateTime.Now;
                message.Sent = true;

                if (_messageService.Add(message))
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetNotiMessage()
        {
            User user = Session["User"] as User;
            var list = _messageService.GetAllNotiAdmin(user.ID).Select(x => new { ID = x.ID, FromUserID = x.FromUserID, FromUserAvatar = x.User.Avatar, FromUserName = x.User.FullName, CreatedDate = (DateTime.Now - x.CreatedDate.Value).Minutes });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}