namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rating")]
    public partial class Rating
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int MemberID { get; set; }

        public int Star { get; set; }

        public string Content { get; set; }

        public virtual Member Member { get; set; }

        public virtual Product Product { get; set; }
    }
}
