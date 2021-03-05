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
        [Display(Name = "Độ tuổi")]
        public int AgeID { set; get; }
        [Display(Name = "Giới tính")]
        public int GenderID { set; get; }
        [Display(Name = "Danh mục")]
        public int CategoryID { set; get; }

        [Display(Name = "Hình ảnh 1")]
        [MaxLength(256)]
        public string Image1 { set; get; }
        [Display(Name = "Hình ảnh 2")]
        [MaxLength(256)]
        public string Image2 { set; get; }
        [Display(Name = "Hình ảnh 3")]
        [MaxLength(256)]
        public string Image3 { set; get; }
        [Display(Name = "Hình ảnh 4")]
        [MaxLength(256)]
        public string Image4 { set; get; }

        public string ClipReview { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào giá bán.")]
        [Display(Name = "Giá bán")]
        public decimal Price { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào giá khuyến mãi.")]
        [Display(Name = "Giá khuyến mãi")]
        public decimal? PromotionPrice { set; get; }
        public int Discount { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào số lượng.")]
        [Display(Name = "Số lượng")]
        public int Quantity { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập vào mô tả sản phẩm.")]
        [Display(Name = "Mô tả sản phẩm")]
        public string Description { set; get; }

        [Display(Name = "Hiển thị trang chủ")]
        public bool HomeFlag { set; get; }
        [Display(Name = "Sản phẩm bán chạy")]
        public bool HotFlag { set; get; }
        public int? ViewCount { set; get; }
        public int? CommentCount { get; set; }
        public int? PurchasedCount { get; set; }
        [Required]
        [Display(Name = "Nhà cung cấp")]
        public int SupplierID { get; set; }
        [Required]
        [Display(Name = "Hãng sản xuất")]
        public int ProducerID { get; set; }
        [Display(Name = "Sản phẩm mới")]
        public bool IsNew { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdatedDate { get; set; }

        [ForeignKey("GenderID")]
        public virtual Gender Gender { set; get; }
        [ForeignKey("AgeID")]
        public virtual Ages Ages { set; get; }
        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { set; get; }
        [ForeignKey("ProducerID")]
        public virtual Producer Producer { set; get; }
    }
}