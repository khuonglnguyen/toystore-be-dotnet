using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("ImportCouponDetail")]
    public class ImportCouponDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int ImportCouponID { get; set; }
        public int ProductID { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ImportCouponID")]
        public virtual ImportCoupon ImportCoupon { set; get; }
        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }
    }
}