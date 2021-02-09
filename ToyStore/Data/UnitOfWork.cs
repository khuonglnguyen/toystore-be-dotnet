using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data.Repository;
using ToyStore.Models;

namespace ToyStore.Data
{
    public class UnitOfWork : IDisposable
    {
        private ToyStoreDbContext DbContext = new ToyStoreDbContext();
        private GenericRepository<Product> productRepository;
        private GenericRepository<ProductCategory> productCategoryRepository;
        private GenericRepository<Supplier> supplierRepository;
        private GenericRepository<Producer> producerRepository;
        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<Product>(DbContext);
                }
                return productRepository;
            }
        }
        public GenericRepository<ProductCategory> ProductCategoryRepository
        {
            get
            {
                if (this.productCategoryRepository == null)
                {
                    this.productCategoryRepository = new GenericRepository<ProductCategory>(DbContext);
                }
                return productCategoryRepository;
            }
        }
        public GenericRepository<Supplier> SupplierRepository
        {
            get
            {
                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new GenericRepository<Supplier>(DbContext);
                }
                return supplierRepository;
            }
        }
        public GenericRepository<Producer> ProducerRepository
        {
            get
            {
                if (this.producerRepository == null)
                {
                    this.producerRepository = new GenericRepository<Producer>(DbContext);
                }
                return producerRepository;
            }
        }
        public void Save()
        {
            DbContext.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}