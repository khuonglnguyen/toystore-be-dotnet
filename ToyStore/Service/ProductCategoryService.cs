﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IProductCategoryService
    {
        ProductCategory AddProductCategory(ProductCategory productCategory);
        IEnumerable<ProductCategory> GetProductCategoryList();
        IEnumerable<ProductCategory> GetProductCategoryList(string keyWord);
        IEnumerable<ProductCategory> GetProductCategoryListName(string keyword);
        ProductCategory GetByID(int ID);
        void UpdateProductCategory(ProductCategory productCategory);
        void DeleteProductCategory(ProductCategory productCategory);
        void MultiDeleteProductCategory(string[] IDs);
        void Save();
    }
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly UnitOfWork context;
        public ProductCategoryService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public ProductCategory AddProductCategory(ProductCategory productCategory)
        {
            productCategory.LastUpdatedDate = DateTime.Now;
            this.context.ProductCategoryRepository.Insert(productCategory);
            return productCategory;
        }

        public void DeleteProductCategory(ProductCategory productCategory)
        {
            productCategory.IsActive = false;
            this.context.ProductCategoryRepository.Delete(productCategory);
        }
        public void MultiDeleteProductCategory(string[] IDs)
        {
            foreach (var id in IDs)
            {
                ProductCategory productCategory = GetByID(int.Parse(id));
                productCategory.IsActive = false;
                UpdateProductCategory(productCategory);
            }
        }

        public ProductCategory GetByID(int ID)
        {
            return this.context.ProductCategoryRepository.GetDataByID(ID);
        }

        public IEnumerable<ProductCategory> GetProductCategoryList()
        {
            IEnumerable<ProductCategory> listProductCategory = this.context.ProductCategoryRepository.GetAllData();
            return listProductCategory;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateProductCategory(ProductCategory productCategory)
        {
            productCategory.LastUpdatedDate = DateTime.Now;
            this.context.ProductCategoryRepository.Update(productCategory);
        }

        public IEnumerable<ProductCategory> GetProductCategoryList(string keyWord)
        {
            IEnumerable<ProductCategory> listProductCategory = this.context.ProductCategoryRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listProductCategory;
        }

        public IEnumerable<ProductCategory> GetProductCategoryListName(string keyword)
        {
            IEnumerable<ProductCategory> listProductCategoryName = this.context.ProductCategoryRepository.GetAllData(x=>x.Name.Contains(keyword));
            return listProductCategoryName;
        }
    }
}