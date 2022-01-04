using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IMessageService
    {
        IEnumerable<Message> GetAllByUserID(int UserID);
        IEnumerable<Message> GetAllNotiAdmin(int AdminSideID);
        List<Message> GetIndexAdmin(int AdminSideID);
        Message GetLastByUserID(int UserID);
        bool Add(Message message);
        bool UpdateSent(int MessageID);
        bool UpdateSentClient();
    }
    public class MessageService : IMessageService
    {
        private readonly UnitOfWork context;
        public MessageService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public bool Add(Message message)
        {
            try
            {
                this.context.MessageRepository.Insert(message);
                return true;
            }
            catch (Exception)
            {
                throw;
                return false;
            }
        }

        public IEnumerable<Message> GetAllNotiAdmin(int AdminSideID)
        {
            IEnumerable<Message> listMessage = this.context.MessageRepository.GetAllData(x => x.Sent == false && x.FromUserID != AdminSideID && x.User.IsDeleted == false);
            return listMessage;
        }

        public IEnumerable<Message> GetAllByUserID(int UserID)
        {
            IEnumerable<Message> listMessage = this.context.MessageRepository.GetAllData(x => x.FromUserID == UserID || x.ToUserID == UserID).OrderBy(x => x.CreatedDate);
            return listMessage;
        }

        public Message GetLastByUserID(int UserID)
        {
            Message message = this.context.MessageRepository.GetAllData(x => x.FromUserID == UserID).OrderBy(x => x.CreatedDate).LastOrDefault();
            return message;
        }

        public bool UpdateSent(int MessageID)
        {
            try
            {
                Message message = this.context.MessageRepository.GetDataByID(MessageID);
                if (!message.Sent.Value)
                {
                    message.Sent = true;
                    this.context.MessageRepository.Update(message);
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateSentClient()
        {
            try
            {
                Message message = this.context.MessageRepository.GetAllData().Last();
                if (!message.Sent.Value)
                {
                    message.Sent = true;
                    this.context.MessageRepository.Update(message);
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Message> GetIndexAdmin(int AdminSideID)
        {
            IEnumerable<User> listUser = this.context.UserRepository.GetAllData(x => x.IsDeleted == false);
            List<Message> messages = new List<Message>();
            foreach (User item in listUser)
            {
                Message message = this.context.MessageRepository.GetAllData(x => x.FromUserID == item.ID && x.FromUserID != AdminSideID && x.User.IsDeleted == false).LastOrDefault();
                if (message != null)
                {
                    messages.Add(message);
                }
            }
            return messages;
        }
    }
}