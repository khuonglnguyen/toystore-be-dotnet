using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ToyStore.Data;

namespace ToyStore.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Image { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Member { set; get; }
        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }
        public Cart(int iID)
        {
            ToyStoreDbContext db = new ToyStoreDbContext();
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
    }
}