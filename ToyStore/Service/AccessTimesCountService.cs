using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        IEnumerable<AccessTimesCount> GetListAccessTimeCountStatistic(DateTime from, DateTime to);
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

        public IEnumerable<AccessTimesCount> GetListAccessTimeCountStatistic(DateTime from, DateTime to)
        {
            IEnumerable<AccessTimesCount> accessTimesCounts = context.AccessTimesCountRepository.GetAllData(x => DbFunctions.TruncateTime(x.Date) >= from.Date && DbFunctions.TruncateTime(x.Date) <= to.Date);
            return accessTimesCounts;
        }
        public int GetSum()
        {
            return context.AccessTimesCountRepository.GetAllData().Sum(x => x.AccessTimes);
        }
    }
}