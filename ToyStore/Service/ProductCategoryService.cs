using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ToyStore.Data;
using ToyStore.Extensions;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IProductCategoryService
    {
        ProductCategory AddProductCategory(ProductCategory productCategory);
        IEnumerable<ProductCategory> GetProductCategoryList();
        IEnumerable<ProductCategory> GetProductCategoryHome();
        ProductCategory GetProductCategoryByName(string Name);
        IEnumerable<ProductCategory> GetProductCategoryList(string keyWord);
        List<string> GetProductCategoryListName(string keyword);
        ProductCategory GetByID(int ID);
        void UpdateProductCategory(ProductCategory productCategory);
        void Block(ProductCategory productCategory);
        void Active(ProductCategory productCategory);
        void MultiDeleteProductCategory(string[] IDs);
        void Save();
        ProductCategory GetByName(string Name);
        bool CheckName(string Name);
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
            productCategory.SEOKeyword = StringHelper.UrlFriendly(productCategory.Name);
            productCategory.LastUpdatedDate = DateTime.Now;
            this.context.ProductCategoryRepository.Insert(productCategory);
            return productCategory;
        }

        public void Block(ProductCategory productCategory)
        {
            productCategory.IsActive = false;
            this.context.ProductCategoryRepository.Update(productCategory);
        }
        public void Active(ProductCategory productCategory)
        {
            productCategory.IsActive = true;
            this.context.ProductCategoryRepository.Update(productCategory);
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
            IEnumerable<ProductCategory> listProductCategory = this.context.ProductCategoryRepository.GetAllData().OrderByDescending(x=>x.LastUpdatedDate);
            return listProductCategory;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateProductCategory(ProductCategory productCategory)
        {
            productCategory.LastUpdatedDate = DateTime.Now;
            IEnumerable<Product> products = this.context.ProductRepository.GetAllData(x => x.CategoryID == productCategory.ID);
            foreach (var item in products)
            {
                if (item.IsActive != productCategory.IsActive)
                {
                    item.IsActive = productCategory.IsActive;
                }
                this.context.ProductRepository.Update(item);
            }
            this.context.ProductCategoryRepository.Update(productCategory);
        }

        public IEnumerable<ProductCategory> GetProductCategoryList(string keyWord)
        {
            IEnumerable<ProductCategory> listProductCategory = this.context.ProductCategoryRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listProductCategory;
        }

        public List<string> GetProductCategoryListName(string keyword)
        {
            IEnumerable<ProductCategory> listProductCategoryName = this.context.ProductCategoryRepository.GetAllData(x => x.Name.Contains(keyword) && x.IsActive == true);
            List<string> names = new List<string>();
            foreach (var item in listProductCategoryName)
            {
                names.Add(item.Name);
            }
            return names;
        }

        public ProductCategory GetProductCategoryByName(string Name)
        {
            ProductCategory productCategory = this.context.ProductCategoryRepository.GetAllData().SingleOrDefault(x => x.Name == Name);
            return productCategory;
        }

        public IEnumerable<ProductCategory> GetProductCategoryHome()
        {
            IEnumerable<ProductCategory> listProductCategory = this.context.ProductCategoryRepository.GetAllData(x=>x.IsActive==true);
            return listProductCategory;
        }
        public ProductCategory GetByName(string Name)
        {
            ProductCategory productCategory = context.ProductCategoryRepository.GetAllData().FirstOrDefault(x => x.Name == Name);
            return productCategory;
        }
        public bool CheckName(string Name)
        {
            var check = context.ProductCategoryRepository.GetAllData(x => x.Name == Name && x.IsActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}