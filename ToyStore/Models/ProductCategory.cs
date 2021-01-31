using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Display(Name = "Tên danh mục")]
        [MaxLength(256)]
        public string Name { set; get; }

        [Display(Name = "Mô tả")]
        [MaxLength(500)]
        public string Description { set; get; }

        [Display(Name = "Hình ảnh")]
        [MaxLength(256)]
        public string Image { set; get; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        [Display(Name = "Ngày cập nhật cuối")]
        public DateTime LastUpdatedDate { get; set; }

        public virtual IEnumerable<Product> Products { set; get; }
    }
}