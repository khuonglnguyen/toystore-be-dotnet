using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IOrderService
    {
        Order AddOrder(Order order);
        IEnumerable<Order> GetAll();
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
        decimal GetTotalRevenue();
        int GetTotalOrder();
        void Update(Order order);
        void UpdateTotal(int OrderID, decimal Total);
        IEnumerable<Order> GetListOrderStatistic(DateTime from, DateTime to);
        bool Paid(int ID);
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
            return context.OrderRepository.GetAllData(x => x.IsDelivere == true && x.IsCancel == false && x.IsReceived == false);
        }

        public IEnumerable<Order> GetOrderDeliveredAndPaid()
        {
            return context.OrderRepository.GetAllData(x => x.IsPaid == true && x.IsDelivere == true && x.IsCancel == false);
        }

        public IEnumerable<Order> GetOrderNotDelivery()
        {
            return context.OrderRepository.GetAllData(x => x.IsDelivere == false && x.IsCancel == false);
        }

        public IEnumerable<Order> GetOrderNotApproval()
        {
            return context.OrderRepository.GetAllData(x => x.IsApproved == false && x.IsCancel == false);
        }

        public void Update(Order order)
        {
            context.OrderRepository.Update(order);
        }

        public IEnumerable<Order> GetByCustomerID(int ID)
        {
            return context.OrderRepository.GetAllData(x => x.UserID == ID);
        }

        public Order Received(int ID)
        {
            Order order = context.OrderRepository.GetDataByID(ID);
            order.IsReceived = true;
            order.IsPaid = true;
            order.DateShip = DateTime.Now;
            context.OrderRepository.Update(order);
            //Update PurchasedCount
            IEnumerable<OrderDetail> orderDetails = context.OrderDetailRepository.GetAllData(x => x.OrderID == ID);
            foreach (var item in orderDetails)
            {
                Product product = context.ProductRepository.GetDataByID(item.ProductID);
                product.PurchasedCount += item.Quantity;
                product.Quantity -= item.Quantity;
                context.ProductRepository.Update(product);
            }
            return order;
        }

        public bool Paid(int ID)
        {
            Order order = context.OrderRepository.GetDataByID(ID);
            if (order != null)
            {
                order.IsPaid = true;
                context.OrderRepository.Update(order);
            }
            return true;
        }

        public decimal GetTotalRevenue()
        {
            return context.OrderDetailRepository.GetAllData(x => x.Order.IsPaid == true).Sum(x => x.Price);
        }

        public int GetTotalOrder()
        {
            return context.OrderRepository.GetAllData().Count();
        }

        public void UpdateTotal(int OrderID, decimal Total)
        {
            Order order = context.OrderRepository.GetDataByID(OrderID);
            order.Total = Total;
            context.OrderRepository.Update(order);
        }

        public IEnumerable<Order> GetListOrderStatistic(DateTime from, DateTime to)
        {
            IEnumerable<OrderDetail> orderDetails = context.OrderDetailRepository.GetAllData(x => DbFunctions.TruncateTime(x.Order.DateOrder) >= from.Date && DbFunctions.TruncateTime(x.Order.DateOrder) <= to.Date);

            List<int> OrderIDs = new List<int>();
            foreach (var item in orderDetails)
            {
                OrderIDs.Add(item.OrderID);
            }
            if (OrderIDs.Count() > 0)
            {
                return context.OrderRepository.GetAllData(x => x.IsReceived == true && OrderIDs.Contains(x.ID)).OrderByDescending(x => x.Total);
            }
            return null;
        }

        public IEnumerable<Order> GetAll()
        {
            return context.OrderRepository.GetAllData();
        }
    }
}