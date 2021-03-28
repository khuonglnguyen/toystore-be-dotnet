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
        IEnumerable<Product> GetProductFilterByAges(int ageID, int min, int max, int discount);
        IEnumerable<Product> GetProductListByCategory(int ProductCategoryID);
        IEnumerable<Product> GetProductListByCategoryParent(int ProductCategoryParentID);
        IEnumerable<Product> GetProductListByGender(int GenderID);
        IEnumerable<Product> GetProductListByAge(int AgeID);
        IEnumerable<Product> GetProductListByProducer(int ProducerID);
        IEnumerable<Product> GetProductListIsNew();
        IEnumerable<Product> GetProductList(string keyWord);
        IEnumerable<Product> GetProductListForManage();
        IEnumerable<Product> GetProductListForManage(string keyWord);
        IEnumerable<Product> GetProductListForHomePage(int productCategoryID);
        IEnumerable<Product> GetProductListForDiscount();
        IEnumerable<Product> GetProductListRandom();
        Product GetByID(int ID);
        void UpdateQuantity(int ID, int Quantity);
        IEnumerable<Product> GetProductListName(string keyword);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void ActiveProduct(Product product);
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
            product.Discount = (int)(product.Price - (((product.Price / product.Price) * product.PromotionPrice)));
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
            return this.context.ProductRepository.GetAllData(x => x.IsActive == true);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Product product)
        {
            product.LastUpdatedDate = DateTime.Now;
            decimal tmp = Convert.ToDecimal(product.Discount) / 100;
            product.PromotionPrice = product.Price - (product.Price * tmp);
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
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.CategoryID == ProductCategoryID && x.IsActive == true);
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
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData().OrderBy(x => Guid.NewGuid()).Take(10);
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

        public IEnumerable<Product> GetProductListByCategoryParent(int ProductCategoryParentID)
        {
            ProductCategoryParent productCategoryParent = this.context.ProductCategoryParentRepository.GetDataByID(ProductCategoryParentID);
            List<Product> listProduct = new List<Product>();
            IEnumerable<ProductCategory> productCategoryList = this.context.ProductCategoryRepository.GetAllData().Where(x => x.ParentID == productCategoryParent.ID);
            foreach (var item in productCategoryList)
            {
                List<Product> products = (List<Product>)this.context.ProductRepository.GetAllData(x => x.CategoryID == item.ID && x.IsActive == true);
                listProduct.AddRange(products);
            }
            return listProduct;
        }

        public IEnumerable<Product> GetProductListByGender(int GenderID)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.GenderID == GenderID && x.IsActive == true);
            return listProduct;
        }

        public IEnumerable<Product> GetProductListIsNew()
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.IsNew == true && x.IsActive == true);
            return listProduct;
        }

        public IEnumerable<Product> GetProductFilterByAges(int ageID, int min, int max, int discount)
        {
            IEnumerable<Product> listProduct;
            if (max != 0)
            {
                listProduct = this.context.ProductRepository.GetAllData(x => x.AgeID == ageID && x.PromotionPrice >= min && x.PromotionPrice <= max && x.IsActive == true);
            }
            else
            {
                listProduct = this.context.ProductRepository.GetAllData(x => x.AgeID == ageID && x.PromotionPrice >= max && x.IsActive == true);
            }
            return listProduct;
        }

        public IEnumerable<Product> GetProductListForManage()
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData();
            return listProduct;
        }

        public IEnumerable<Product> GetProductListForManage(string keyWord)
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listProduct;
        }

        public void ActiveProduct(Product product)
        {
            product.IsActive = true;
            this.context.ProductRepository.Update(product);
        }

        public void UpdateQuantity(int ID, int Quantity)
        {
            Product product = context.ProductRepository.GetDataByID(ID);
            product.Quantity -= Quantity;
            context.ProductRepository.Update(product);
        }
    }
}