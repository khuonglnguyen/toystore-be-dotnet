namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductViewed")]
    public partial class ProductViewed
    {
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int ProductID { get; set; }

        public DateTime Date { get; set; }

        public virtual Member Member { get; set; }

        public virtual Product Product { get; set; }
    }
}
