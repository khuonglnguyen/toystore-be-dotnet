using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Xml.Linq;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ToyStoreDbContext DbContext;
        private IDbSet<T> dbEntity;
        public GenericRepository(ToyStoreDbContext DbContext)
        {
            this.DbContext = DbContext;
            dbEntity = DbContext.Set<T>();
        }
        public void Insert(T Model)
        {
            dbEntity.Add(Model);
            DbContext.SaveChanges();
        }
        public IEnumerable<T> GetAllData()
        {
            return (IEnumerable<T>)dbEntity.ToList();
        }
        public T GetDataByID(int ID)
        {
            return dbEntity.Find(ID);
        }
        public void Update(T Model)
        {
            DbContext.Entry(Model).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
        public void Delete(T Model)
        {
            DbContext.Entry(Model).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
        public void Save()
        {
            DbContext.SaveChanges();
        }
    }
}