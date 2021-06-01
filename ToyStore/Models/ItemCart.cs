//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ItemCart
    {
        public ItemCart(int iID)
        {
            ToyStore2021Entities db = new ToyStore2021Entities();
            this.ProductID = iID;
            Product product = db.Products.Single(n => n.ID == iID);
            this.Name = product.Name;
            this.Image = product.Image1;
            this.Price = (decimal)product.PromotionPrice;
            this.Quantity = 1;
            this.Total = Price * Quantity;
        }
        public ItemCart()
        {

        }
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

        public virtual Member Member { get; set; }
        public virtual Product Product { get; set; }
    }
}
