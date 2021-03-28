using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToyStore.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public int CustomerID { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime DateShip { get; set; }
        public int Offer { get; set; }
        public bool IsApproved { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCancel { get; set; }
        public bool IsDelete { get; set; }
        public bool IsDelivere { get; set; }
        public bool IsReceived { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { set; get; }
    }
}