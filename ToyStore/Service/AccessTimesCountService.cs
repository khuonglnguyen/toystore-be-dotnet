using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IAccessTimesCountService
    {
        int GetSum();
        void AddCount(DateTime Date);
    }
    public class AccessTimesCountService : IAccessTimesCountService
    {
        private readonly UnitOfWork context;
        public AccessTimesCountService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void AddCount(DateTime Date)
        {
            AccessTimesCount accessTimesCount = (AccessTimesCount)context.AccessTimesCountRepository.GetAllData(x => x.Date.Date == Date.Date);
            accessTimesCount.AccessTimes += 1;
            context.AccessTimesCountRepository.Update(accessTimesCount);
        }

        public int GetSum()
        {
            return context.AccessTimesCountRepository.GetAllData().Sum(x => x.AccessTimes);
        }
    }
}