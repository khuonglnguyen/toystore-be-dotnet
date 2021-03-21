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
        void Save();
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

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}