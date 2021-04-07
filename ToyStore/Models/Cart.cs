namespace ToyStore.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        public Cart(int iID)
        {
            ToyStoreModel db = new ToyStoreModel();
            this.ProductID = iID;
            Product product = db.Products.Single(n => n.ID == iID);
            this.Name = product.Name;
            this.Image = product.Image1;
            this.Price = (decimal)product.PromotionPrice;
            this.Quantity = 1;
            this.Total = Price * Quantity;
        }
        public Cart()
        {

        }
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int ProductID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }

        public string Image { get; set; }

        public virtual Member Member { get; set; }

        public virtual Product Product { get; set; }
    }
}
