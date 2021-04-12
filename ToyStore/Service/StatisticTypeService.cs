using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IStatisticTypeService
    {
        IEnumerable<StatisticType> GetStatisticTypeList();
    }
    public class StatisticTypeService : IStatisticTypeService
    {
        private readonly UnitOfWork context;
        public StatisticTypeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public IEnumerable<StatisticType> GetStatisticTypeList()
        {
            return context.StatisticTypeRepository.GetAllData();
        }
    }
}