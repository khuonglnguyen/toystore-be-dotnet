namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImportCouponDetail")]
    public partial class ImportCouponDetail
    {
        public int ID { get; set; }

        public int ImportCouponID { get; set; }

        public int ProductID { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public virtual ImportCoupon ImportCoupon { get; set; }

        public virtual Product Product { get; set; }
    }
}
