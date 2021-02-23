﻿using System;
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
        IEnumerable<Product> GetProductListByCategory(int ProductCategoryID);
        IEnumerable<Product> GetProductListByAge(int AgeID);
        IEnumerable<Product> GetProductListByProducer(int ProducerID);
        IEnumerable<Product> GetProductList(string keyWord);
        IEnumerable<Product> GetProductListForHomePage(int productCategoryID);
        IEnumerable<Product> GetProductListForDiscount();
        IEnumerable<Product> GetProductListRandom();
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
            IEnumerable<Product> listProductName = this.context.ProductRepository.GetAllData(x => x.Name.Contains(keyword) && x.IsActive == true);
            return listProductName;
        }

        public IEnumerable<Product> GetProductList()
        {
            return this.context.ProductRepository.GetAllData(x=>x.IsActive == true);
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
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.Name.Contains(keyWord) && x.IsActive == true);
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

        public IEnumerable<Product> GetProductListByCategory(int ProductCategoryID)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.CategoryID == ProductCategoryID && x.IsActive==true);
            return listProduct;
        }
        public IEnumerable<Product> GetProductListForHomePage(int productCategoryID)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData()
                .Where(x => x.CategoryID == productCategoryID && x.IsActive == true)
                .OrderByDescending(x => x.LastUpdatedDate)
                .Take(3);
            return listProduct;
        }

        public IEnumerable<Product> GetProductListForDiscount()
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData()
                .Where(x => x.HomeFlag == true)
                .OrderByDescending(x => x.LastUpdatedDate)
                .Take(10);
            return listProduct;
        }

        public IEnumerable<Product> GetProductListRandom()
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData().OrderBy(x=>Guid.NewGuid()).Take(10);
            return listProduct;
        }

        public IEnumerable<Product> GetProductListByAge(int AgeID)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.AgeID == AgeID && x.IsActive == true);
            return listProduct;
        }

        public IEnumerable<Product> GetProductListByProducer(int ProducerID)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.ProducerID == ProducerID && x.IsActive == true);
            return listProduct;
        }
    }
}