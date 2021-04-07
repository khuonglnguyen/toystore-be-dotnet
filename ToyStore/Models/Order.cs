namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ID { get; set; }

        public int CustomerID { get; set; }

        public DateTime DateOrder { get; set; }

        public DateTime DateShip { get; set; }

        public int Offer { get; set; }

        public bool IsPaid { get; set; }

        public bool IsCancel { get; set; }

        public bool IsDelete { get; set; }

        public bool IsDelivere { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReceived { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
