namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QA")]
    public partial class QA
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int MemberID { get; set; }

        public bool Status { get; set; }

        [StringLength(1000)]
        public string Question { get; set; }

        [StringLength(1000)]
        public string Answer { get; set; }

        public DateTime? DateQuestion { get; set; }

        public DateTime? DateAnswer { get; set; }

        public int EmloyeeID { get; set; }

        public virtual Emloyee Emloyee { get; set; }

        public virtual Member Member { get; set; }

        public virtual Product Product { get; set; }
    }
}
