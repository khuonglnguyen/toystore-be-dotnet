namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccessTimesCount")]
    public partial class AccessTimesCount
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public int AccessTimes { get; set; }
    }
}
