
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
    
public partial class Rating
{

    public int ID { get; set; }

    public int ProductID { get; set; }

    public int MemberID { get; set; }

    public int Star { get; set; }

    public string Content { get; set; }



    public virtual Member Member { get; set; }

    public virtual Product Product { get; set; }

}

}
