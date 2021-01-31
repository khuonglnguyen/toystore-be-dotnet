using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IProductService
    {
        Product AddProduct(Product product);
        List<Product> GetProductList();
        Product GetByID(int ID);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void Save();
    }
    public class ProductService : IProductService
    {
        private readonly UnitOfWork context;
        public ProductService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Product AddProduct(Product product)
        {
            this.context.ProductRepository.Insert(product);
            return product;
        }

        public void DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Product GetByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductList()
        {
            return this.context.ProductRepository.GetAllData().ToList();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}