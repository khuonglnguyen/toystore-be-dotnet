using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("ImportCoupon")]
    public class ImportCoupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int ProducerID { get; set; }
        public DateTime Date { get; set; }
        public bool IsDelete { get; set; }
        [ForeignKey("ProducerID")]
        public virtual Producer Producer { set; get; }
    }
}