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
        Emloyee Add(Emloyee emloyee);
        Emloyee CheckLogin(int id, string password);
        IEnumerable<Emloyee> GetList(int ID);
        int GetTotalEmloyee();
        List<string> GetEmloyeeListName(string keyword);
        Emloyee GetByID(int ID);
        void Update(Emloyee emloyee);
        void Block(Emloyee emloyee);
        void Active(Emloyee emloyee);
    }
    public class EmloyeeService : IEmloyeeService
    {
        private readonly UnitOfWork context;
        public EmloyeeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void Active(Emloyee emloyee)
        {
            emloyee.IsActive = true;
            this.context.EmloyeeRepository.Update(emloyee);
        }

        public Emloyee Add(Emloyee emloyee)
        {
            emloyee.Password = "11111111";
            this.context.EmloyeeRepository.Insert(emloyee);
            return emloyee;
        }

        public void Block(Emloyee emloyee)
        {
            emloyee.IsActive = false;
            this.context.EmloyeeRepository.Update(emloyee);
        }

        public Emloyee CheckLogin(int id, string password)
        {
            Emloyee emloyee = this.context.EmloyeeRepository.GetAllData().SingleOrDefault(x => x.ID == id && x.Password == password);
            return emloyee;
        }

        public Emloyee GetByID(int ID)
        {
            return this.context.EmloyeeRepository.GetDataByID(ID);
        }

        public List<string> GetEmloyeeListName(string keyword)
        {
            IEnumerable<Emloyee> listProductName = this.context.EmloyeeRepository.GetAllData(x => x.FullName.Contains(keyword) && x.IsActive == true);
            List<string> names = new List<string>();
            foreach (var item in listProductName)
            {
                names.Add(item.FullName);
            }
            return names;
        }

        public IEnumerable<Emloyee> GetList(int ID)
        {
            return context.EmloyeeRepository.GetAllData(x=>x.ID!=2 && x.ID!=ID);
        }

        public int GetTotalEmloyee()
        {
            return context.EmloyeeRepository.GetAllData().Count();
        }

        public void Update(Emloyee emloyee)
        {
            this.context.EmloyeeRepository.Update(emloyee);
        }
    }
}