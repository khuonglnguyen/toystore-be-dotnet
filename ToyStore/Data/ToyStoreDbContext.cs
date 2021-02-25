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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}