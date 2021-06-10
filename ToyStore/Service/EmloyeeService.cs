using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IEmloyeeService
    {
        Emloyee CheckLogin(int id, string password);
        IEnumerable<Emloyee> GetList();
        int GetTotalEmloyee();
    }
    public class EmloyeeService : IEmloyeeService
    {
        private readonly UnitOfWork context;
        public EmloyeeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Emloyee CheckLogin(int id, string password)
        {
            Emloyee emloyee = this.context.EmloyeeRepository.GetAllData().SingleOrDefault(x => x.ID == id && x.Password == password);
            return emloyee;
        }

        public IEnumerable<Emloyee> GetList()
        {
            return context.EmloyeeRepository.GetAllData();
        }

        public int GetTotalEmloyee()
        {
            return context.EmloyeeRepository.GetAllData().Count();
        }
    }
}