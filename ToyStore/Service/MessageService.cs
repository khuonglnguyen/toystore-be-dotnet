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
        //IEnumerable<Message> GetByFromID(int FromID);
        bool Add(Message message);
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
                return false;
            }
        }

        //public IEnumerable<Message> GetByFromID(int FromID)
        //{
        //    IEnumerable<Message> listMessage = this.context.MessageRepository.GetAllData(x => x.FromID == FromID).OrderBy(x=>x.CreatedDate);
        //    return listMessage;
        //}
    }
}