using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
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
        public void Insert(Models.Product product)
        {
            DbContext.Products.Add(product);
            DbContext.SaveChanges();
        }
        public IEnumerable<Models.Product> GetAll()
        {
            return DbContext.Products.ToList();
        }
        public Product GetByID(int ID)
        {
            return DbContext.Products.Find(ID);
        }
        public void Update(Models.Product product)
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
            throw new NotImplementedException();
        }
    }
}