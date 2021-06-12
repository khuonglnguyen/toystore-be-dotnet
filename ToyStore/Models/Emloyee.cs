
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace ToyStore.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Emloyee
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Emloyee()
    {

        this.DiscountCodes = new HashSet<DiscountCode>();

        this.ImportCoupons = new HashSet<ImportCoupon>();

        this.QAs = new HashSet<QA>();

    }


    public int ID { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Image { get; set; }

    public int EmloyeeTypeID { get; set; }

    public Nullable<bool> IsActive { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<DiscountCode> DiscountCodes { get; set; }

    public virtual EmloyeeType EmloyeeType { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ImportCoupon> ImportCoupons { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<QA> QAs { get; set; }

}

}
