using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IOrderService
    {
        Order AddOrder(Order order);
        void Save();
    }
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork context;
        public OrderService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Order AddOrder(Order order)
        {
            this.context.OrderRepository.Insert(order);
            return order;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}