using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IEmloyeeTypeService
    {
        EmloyeeType GetEmloyeeTypeByID(int ID);
    }
    public class EmloyeeTypeService : IEmloyeeTypeService
    {
        private readonly UnitOfWork context;
        public EmloyeeTypeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public EmloyeeType GetEmloyeeTypeByID(int ID)
        {
            return context.EmloyeeTypeRepository.GetDataByID(ID);
        }
    }
}