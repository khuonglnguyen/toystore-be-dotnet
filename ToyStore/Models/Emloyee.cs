namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Emloyee")]
    public partial class Emloyee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Emloyee()
        {
            Decentralizations = new HashSet<Decentralization>();
            ImportCoupons = new HashSet<ImportCoupon>();
            QAs = new HashSet<QA>();
        }

        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Image { get; set; }

        public int EmloyeeTypeID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Decentralization> Decentralizations { get; set; }

        public virtual EmloyeeType EmloyeeType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportCoupon> ImportCoupons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QA> QAs { get; set; }
    }
}
