using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface ICustomerService
    {
        Customer AddCustomer(Customer customer);
        IEnumerable<Customer> GetAll();
        string GetEmailByID(int ID);
        Customer GetByEmail(string Email);
        void Update(Customer customer);
        void Save();
        bool CheckEmail(string Email);
    }
    public class CustomerService : ICustomerService
    {
        private readonly UnitOfWork context;
        public CustomerService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Customer AddCustomer(Customer customer)
        {
            this.context.CustomerRepository.Insert(customer);
            return customer;
        }

        public IEnumerable<Customer> GetAll()
        {
            return context.CustomerRepository.GetAllData();
        }

        public string GetEmailByID(int ID)
        {
            return context.CustomerRepository.GetDataByID(ID).Email;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Customer customer)
        {
            context.CustomerRepository.Update(customer);
        }

        public bool CheckEmail(string Email)
        {
            var check = context.CustomerRepository.GetAllData(x => x.Email == Email);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public Customer GetByEmail(string Email)
        {
            return context.CustomerRepository.GetAllData(x=>x.Email == Email).FirstOrDefault();
        }
    }
}