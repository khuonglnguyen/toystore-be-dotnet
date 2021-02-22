using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IAgeService
    {
        IEnumerable<Ages> GetAgeList();
        Ages GetAgeByID(int ID);
    }
    public class AgeService : IAgeService
    {
        private readonly UnitOfWork context;
        public AgeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public IEnumerable<Ages> GetAgeList()
        {
            IEnumerable<Ages> listAge = this.context.AgeRepository.GetAllData();
            return listAge;
        }

        public Ages GetAgeByID(int ID)
        {
            return this.context.AgeRepository.GetDataByID(ID);
        }
    }
}