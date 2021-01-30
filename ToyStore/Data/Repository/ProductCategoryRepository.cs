using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ToyStore.Models;

namespace ToyStore.Data.Repository
{
    public class ProductCategoryRepository : IProductCategory
    {
        private ToyStoreDbContext DbContext;
        public ProductCategoryRepository(ToyStoreDbContext objempcontext)
        {
            this.DbContext = objempcontext;
        }
        public void Insert(ProductCategory productCategory)
        {
            DbContext.ProductCategories.Add(productCategory);
            DbContext.SaveChanges();
        }
        public IEnumerable<ProductCategory> GetAll()
        {
            return (IEnumerable<ProductCategory>)DbContext.ProductCategories.ToList();
        }
        public ProductCategory GetByID(int ID)
        {
            return DbContext.ProductCategories.Find(ID);
        }
        public void Update(ProductCategory productCategory)
        {
            DbContext.Entry(productCategory).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
        public void Delete(int ID)
        {
            ProductCategory productCategory = DbContext.ProductCategories.Find(ID);
            DbContext.ProductCategories.Remove(productCategory);
            DbContext.SaveChanges();
        }
        public void Save()
        {
            DbContext.SaveChanges();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}