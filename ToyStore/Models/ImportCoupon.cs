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
        public int SupplierID { get; set; }
        public int EmloyeeID { get; set; }
        public DateTime Date { get; set; }
        public bool IsDelete { get; set; }
        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { set; get; }
        [ForeignKey("EmloyeeID")]
        public virtual Emloyee Emloyee { set; get; }
    }
}