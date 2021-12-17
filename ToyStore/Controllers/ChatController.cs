using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyStore.Service;
using ToyStore.Models;

namespace ToyStore.Controllers
{
    public class ChatController : Controller
    {
        private IMessageService _messageService;
        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        // GET: Message
        //[HttpGet]
        //public JsonResult GetMessageClient(int FromID)
        //{
        //    var listMessage = _messageService.GetByFromID(FromID).Select(x => new { Content = x.Content, CreatedDate = x.CreatedDate.Value, FromName = x.Us.FullName, ToName = x.Emloyee.FullName , AvatarMember = x.Member.Avatar});
        //    return Json(listMessage, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult Send(int FromID, int ToID, string Content)
        //{
        //    Message message = new Message();
        //    message.FromID = FromID;
        //    message.ToID = ToID;
        //    message.Content = Content;
        //    message.CreatedDate = DateTime.Now;

        //    if (_messageService.Add(message))
        //    {
        //        return Json(new
        //        {
        //            status = true
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new
        //    {
        //        status = false
        //    }, JsonRequestBehavior.AllowGet);
        //}
    }
}