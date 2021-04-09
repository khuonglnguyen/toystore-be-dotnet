using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ToyStore.Models
{
    public partial class ToyStoreModel : DbContext
    {
        public ToyStoreModel()
            : base("name=ToyStoreModel")
        {
        }

        public virtual DbSet<AccessTimesCount> AccessTimesCounts { get; set; }
        public virtual DbSet<Age> Ages { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Decentralization> Decentralizations { get; set; }
        public virtual DbSet<Emloyee> Emloyees { get; set; }
        public virtual DbSet<EmloyeeType> EmloyeeTypes { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<ImportCoupon> ImportCoupons { get; set; }
        public virtual DbSet<ImportCouponDetail> ImportCouponDetails { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberCategory> MemberCategories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductCategoryParent> ProductCategoryParents { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductViewed> ProductVieweds { get; set; }
        public virtual DbSet<QA> QAs { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImportCoupon>()
                .HasMany(e => e.ImportCouponDetails)
                .WithRequired(e => e.ImportCoupon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductCategory)
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<ProductCategoryParent>()
                .HasMany(e => e.ProductCategories)
                .WithRequired(e => e.ProductCategoryParent)
                .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ImportCouponDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);
        }
    }
}
