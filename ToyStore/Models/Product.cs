namespace ToyStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Carts = new HashSet<Cart>();
            Comments = new HashSet<Comment>();
            ImportCouponDetails = new HashSet<ImportCouponDetail>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductVieweds = new HashSet<ProductViewed>();
            QAs = new HashSet<QA>();
            Ratings = new HashSet<Rating>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int CategoryID { get; set; }

        [StringLength(256)]
        public string Image1 { get; set; }

        [StringLength(256)]
        public string Image2 { get; set; }

        [StringLength(256)]
        public string Image3 { get; set; }

        [StringLength(256)]
        public string Image4 { get; set; }

        public string ClipReview { get; set; }

        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public bool HomeFlag { get; set; }

        public bool HotFlag { get; set; }

        public int? ViewCount { get; set; }

        public int? CommentCount { get; set; }

        public int? PurchasedCount { get; set; }

        public int SupplierID { get; set; }

        public int ProducerID { get; set; }

        public bool IsNew { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int AgeID { get; set; }

        public int GenderID { get; set; }

        public int Discount { get; set; }

        public virtual Age Age { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual Gender Gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportCouponDetail> ImportCouponDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Producer Producer { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductViewed> ProductVieweds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QA> QAs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
