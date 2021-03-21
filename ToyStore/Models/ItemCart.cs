using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Service;

namespace ToyStore.Models
{
    public class ItemCart
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Image { get; set; }
        public ItemCart(int iID)
        {
            ToyStoreDbContext db = new ToyStoreDbContext();
            this.ID = iID;
            Product product = db.Products.Single(n => n.ID == iID);
            this.Name = product.Name;
            this.Image = product.Image1;
            this.Price = (decimal)product.PromotionPrice;
            this.Quantity = 1;
            this.Total = Price * Quantity;
        }
        public ItemCart(int iID, int Quantity)
        {
            ToyStoreDbContext db = new ToyStoreDbContext();
            this.ID = iID;
            Product product = db.Products.Single(n => n.ID == iID);
            this.Name = product.Name;
            this.Image = product.Image1;
            this.Price = product.Price;
            this.Quantity = Quantity;
            this.Total = Price * Quantity;
        }
        public ItemCart()
        {

        }
    }
}