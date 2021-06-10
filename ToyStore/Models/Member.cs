
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
    
public partial class Member
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Member()
    {

        this.ItemCarts = new HashSet<ItemCart>();

        this.ProductVieweds = new HashSet<ProductViewed>();

        this.QAs = new HashSet<QA>();

        this.Ratings = new HashSet<Rating>();

        this.MemberDiscountCodes = new HashSet<MemberDiscountCode>();

    }


    public int ID { get; set; }

    public int MemberTypeID { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public bool EmailConfirmed { get; set; }

    public string Capcha { get; set; }

    public Nullable<decimal> AmountPurchased { get; set; }

    public string Avatar { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ItemCart> ItemCarts { get; set; }

    public virtual MemberType MemberType { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ProductViewed> ProductVieweds { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<QA> QAs { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Rating> Ratings { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MemberDiscountCode> MemberDiscountCodes { get; set; }

}

}
