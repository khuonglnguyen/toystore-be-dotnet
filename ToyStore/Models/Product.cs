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

        [Required(ErrorMessage = "Vui lòng nhập vào tên sản phẩm.")]
        [Display(Name = "Tên sản phẩm")]
        [MaxLength(256)]
        public string Name { set; get; }

        public int CategoryID { set; get; }

        [MaxLength(256)]
        public string Image1 { set; get; }
        [MaxLength(256)]
        public string Image2 { set; get; }
        [MaxLength(256)]
        public string Image3 { set; get; }
        [MaxLength(256)]
        public string Image4 { set; get; }

        public string ClipReview { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào giá bán.")]
        [Display(Name = "Giá bán")]
        public decimal Price { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào giá khuyến mãi.")]
        [Display(Name = "Giá khuyến mãi")]
        public decimal? PromotionPrice { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào số lượng.")]
        [Display(Name = "Số lượng")]
        public int Quantity { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào mô tả sản phẩm.")]
        [Display(Name = "Mô tả sản phẩm")]
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
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { set; get; }
        [ForeignKey("ProducerID")]
        public virtual Producer Producer { set; get; }
    }
}