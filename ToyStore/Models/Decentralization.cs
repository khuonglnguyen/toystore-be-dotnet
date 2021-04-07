namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Decentralization")]
    public partial class Decentralization
    {
        public int ID { get; set; }

        public int RoleID { get; set; }

        public string Note { get; set; }

        public int EmloyeeID { get; set; }

        public virtual Emloyee Emloyee { get; set; }

        public virtual Role Role { get; set; }
    }
}
