namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImportCoupon")]
    public partial class ImportCoupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImportCoupon()
        {
            ImportCouponDetails = new HashSet<ImportCouponDetail>();
        }

        public int ID { get; set; }

        public DateTime Date { get; set; }

        public bool IsDelete { get; set; }

        public int EmloyeeID { get; set; }

        public int SupplierID { get; set; }

        public virtual Emloyee Emloyee { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportCouponDetail> ImportCouponDetails { get; set; }
    }
}
