﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Data.Repository
{
    public class ProductRepository : IProduct
    {
        private ToyStoreDbContext DbContext;
        public ProductRepository(ToyStoreDbContext objempcontext)
        {
            this.DbContext = objempcontext;
        }
        public void Insert(Product product)
        {
            DbContext.Products.Add(product);
            DbContext.SaveChanges();
        }
        public IEnumerable<Product> GetAll()
        {
            return (IEnumerable<Product>)DbContext.Products.ToList();
        }
        public Product GetByID(int ID)
        {
            return DbContext.Products.Find(ID);
        }
        public void Update(Product product)
        {
            DbContext.Entry(product).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
        public void Delete(int ID)
        {
            Product product = DbContext.Products.Find(ID);
            DbContext.Products.Remove(product);
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