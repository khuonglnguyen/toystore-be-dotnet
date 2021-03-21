using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IOrderDetailService
    {
        OrderDetail AddOrderDetail(OrderDetail order);
        void Save();
    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly UnitOfWork context;
        public OrderDetailService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public OrderDetail AddOrderDetail(OrderDetail orderDetail)
        {
            this.context.OrderDetailRepository.Insert(orderDetail);
            return orderDetail;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}