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
        IEnumerable<Order> GetOrderNotApproval();
        IEnumerable<Order> GetOrderNotDelivery();
        IEnumerable<Order> GetOrderDeliveredAndPaid();
        IEnumerable<Order> ApprovedAndNotDelivery();
        IEnumerable<Order> GetDelivered();
        Order GetByID(int ID);
        IEnumerable<Order> GetByCustomerID(int ID);
        Order Approved(int ID);
        Order Delivered(int ID);
        Order Received(int ID);
        void Update(Order order);
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

        public Order Approved(int ID)
        {
            Order order = context.OrderRepository.GetDataByID(ID);
            order.IsApproved = true;
            context.OrderRepository.Update(order);
            return order;
        }

        public IEnumerable<Order> ApprovedAndNotDelivery()
        {
            return context.OrderRepository.GetAllData(x => x.IsApproved == true && x.IsDelivere == false);
        }

        public Order Delivered(int ID)
        {
            Order order = context.OrderRepository.GetDataByID(ID);
            order.IsDelivere = true;
            context.OrderRepository.Update(order);
            return order;
        }

        public Order GetByID(int ID)
        {
            return context.OrderRepository.GetDataByID(ID);
        }

        public IEnumerable<Order> GetDelivered()
        {
            return context.OrderRepository.GetAllData(x => x.IsDelivere == true);
        }

        public IEnumerable<Order> GetOrderDeliveredAndPaid()
        {
            return context.OrderRepository.GetAllData(x => x.IsPaid == true && x.IsDelivere == true);
        }

        public IEnumerable<Order> GetOrderNotDelivery()
        {
            return context.OrderRepository.GetAllData(x => x.IsDelivere == false);
        }

        public IEnumerable<Order> GetOrderNotApproval()
        {
            return context.OrderRepository.GetAllData(x => x.IsApproved == false);
        }

        public void Update(Order order)
        {
            context.OrderRepository.Update(order);
        }

        public IEnumerable<Order> GetByCustomerID(int ID)
        {
            return context.OrderRepository.GetAllData(x => x.CustomerID == ID);
        }

        public Order Received(int ID)
        {
            Order order = context.OrderRepository.GetDataByID(ID);
            order.IsReceived = true;
            order.IsPaid = true;
            context.OrderRepository.Update(order);
            //Update PurchasedCount
            IEnumerable<OrderDetail> orderDetails = context.OrderDetailRepository.GetAllData(x => x.OrderID == ID);
            foreach (var item in orderDetails)
            {
                Product product = context.ProductRepository.GetDataByID(item.ProductID);
                product.PurchasedCount += item.Quantity;
                context.ProductRepository.Update(product);
            }
            return order;
        }
    }
}