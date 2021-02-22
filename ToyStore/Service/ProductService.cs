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
        IEnumerable<Product> GetProductList();
        IEnumerable<Product> GetProductListWithCategory(int ProductCategoryID);
        IEnumerable<Product> GetProductList(string keyWord);
        Product GetByID(int ID);
        IEnumerable<Product> GetProductListName(string keyword);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void MultiDeleteProduct(string[] IDs);
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
            product.LastUpdatedDate = DateTime.Now;
            this.context.ProductRepository.Insert(product);
            return product;
        }

        public void DeleteProduct(Product product)
        {
            product.IsActive = false;
            this.context.ProductRepository.Delete(product);
        }

        public Product GetByID(int ID)
        {
            return this.context.ProductRepository.GetDataByID(ID);
        }

        public IEnumerable<Product> GetProductListName(string keyword)
        {
            IEnumerable<Product> listProductName = this.context.ProductRepository.GetAllData(x => x.Name.Contains(keyword));
            return listProductName;
        }

        public IEnumerable<Product> GetProductList()
        {
            return this.context.ProductRepository.GetAllData();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            product.LastUpdatedDate = DateTime.Now;
            this.context.ProductRepository.Update(product);
        }

        public IEnumerable<Product> GetProductList(string keyWord)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listProduct;
        }

        public void MultiDeleteProduct(string[] IDs)
        {
            foreach (var id in IDs)
            {
                Product product = GetByID(int.Parse(id));
                product.IsActive = false;
                UpdateProduct(product);
            }
        }

        public IEnumerable<Product> GetProductListWithCategory(int ProductCategoryID)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.CategoryID == ProductCategoryID);
            return listProduct;
        }
    }
}