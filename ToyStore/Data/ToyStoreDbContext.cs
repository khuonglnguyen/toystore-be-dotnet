using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToyStore.Models;

namespace ToyStore.Data
{
    public class ToyStoreDbContext : DbContext
    {
        public ToyStoreDbContext() : base("name=ToyStoreConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Ages> Ages { get; set; }
        public DbSet<ProductCategoryParent> ProductCategoryParents { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<MemberCategory> MemberCategories { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ImportCoupon> ImportCoupons { get; set; }
        public DbSet<ImportCouponDetail> ImportCouponDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<QA> QAs { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Decentralization> Decentralizations { get; set; }
        public DbSet<Emloyee> Emloyees { get; set; }
        public DbSet<EmloyeeType> EmloyeeTypes { get; set; }
        public DbSet<AccessTimesCount> AccessTimesCounts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}