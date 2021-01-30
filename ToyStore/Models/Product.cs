using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        public int CategoryID { set; get; }

        [MaxLength(256)]
        public string Image { set; get; }

        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }
        public int Quantity { set; get; }

        [MaxLength(500)]
        public string Description { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }
        public int? CommentCount { get; set; }
        public int? PurchasedCount { get; set; }
        [Required]
        public int SupplierID { get; set; }
        [Required]
        public int ProducerID { get; set; }
        public bool New { get; set; }
        public bool Deleted { get; set; }

        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { set; get; }
        [ForeignKey("ProducerID")]
        public virtual Producer Producer { set; get; }
    }
}