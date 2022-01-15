using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using ToyStore.Data;
using ToyStore.Extensions;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IProductService
    {
        Product AddProduct(Product product);
        void AddViewCount(int ID);
        int GetTotalProduct();
        int GetTotalProductPurchased();
        IEnumerable<Product> GetProductList();
        IEnumerable<Product> GetProductFilterByAges(int ageID, int min, int max, int discount);
        IEnumerable<Product> GetProductListByCategory(int ProductCategoryID);
        IEnumerable<Product> GetProductListByCategoryParent(int ProductCategoryParentID);
        IEnumerable<Product> GetProductListByGender(int GenderID);
        IEnumerable<Product> GetProductListByAge(int AgeID);
        IEnumerable<Product> GetProductListByProducer(int ProducerID);
        IEnumerable<Product> GetProductListLast();
        IEnumerable<Product> GetProductListViewedByUserID(int UserID);
        IEnumerable<Product> GetProductList(string keyWord);
        IEnumerable<Product> GetProductListForManage();
        IEnumerable<Product> GetProductListForManage(string keyWord);
        IEnumerable<Product> GetProductListForHomePage(int productCategoryID);
        IEnumerable<Product> GetProductListIndex();
        IEnumerable<Product> GetProductListDiscount();
        IEnumerable<Product> GetProductListRandom();
        IEnumerable<Product> GetProductListAlmostOver();
        IEnumerable<Product> GetProductListStocking();
        IEnumerable<Product> GetProductListSold(DateTime from, DateTime to);
        Product GetByID(int ID);
        void UpdateQuantity(int ID, int Quantity);
        void UpdatePurchasedCount(int ID, int PurchasedCount);
        List<string> GetProductListName(string keyword);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void ActiveProduct(Product product);
        void MultiDeleteProduct(string[] IDs);
        void Save();
        Product GetByName(string Name);
        bool CheckName(string Name);
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
            product.PromotionPrice = product.Price - (product.Price / 100 * product.Discount);
            product.Quantity = 0;
            product.ViewCount = 0;
            product.PurchasedCount = 0;
            product.SEOKeyword = StringHelper.UrlFriendly(product.Name);
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

        public List<string> GetProductListName(string keyword)
        {
            IEnumerable<Product> listProductName = this.context.ProductRepository.GetAllData(x => x.Name.Contains(keyword) && x.IsActive == true);
            List<string> names = new List<string>();
            foreach (var item in listProductName)
            {
                names.Add(item.Name);
            }
            return names;
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
            product.PromotionPrice = product.Price - (product.Price / 100 * product.Discount);
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

        public IEnumerable<Product> GetProductListIndex()
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData()
                .Where(x => x.IsActive == true && x.Quantity > 0 && x.PurchasedCount > 0)
                .OrderByDescending(x => x.PurchasedCount)
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

        public IEnumerable<Product> GetProductListLast()
        {
            IEnumerable<Product> listProduct = this.context.ProductRepository.GetAllData(x=>x.IsActive == true).OrderByDescending(x=>x.LastUpdatedDate);
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

        public void UpdatePurchasedCount(int ID, int PurchasedCount)
        {
            Product product = context.ProductRepository.GetDataByID(ID);
            product.PurchasedCount += PurchasedCount;
            context.ProductRepository.Update(product);
        }

        public IEnumerable<Product> GetProductListAlmostOver()
        {
            return context.ProductRepository.GetAllData().OrderBy(x => x.Quantity);
        }

        public void AddViewCount(int ID)
        {
            Product product = context.ProductRepository.GetDataByID(ID);
            product.ViewCount += 1;
            context.ProductRepository.Update(product);
        }

        public IEnumerable<Product> GetProductListViewedByUserID(int UserID)
        {
            var productIDList = context.ProductViewedRepository.GetAllData(x => x.UserID == UserID).OrderByDescending(x => x.Date).Select(x => x.ProductID);
            List<Product> productsList = new List<Product>();
            foreach (var item in productIDList)
            {
                productsList.Add(context.ProductRepository.GetDataByID(item));
            }
            return productsList;
        }

        public int GetTotalProduct()
        {
            return context.ProductRepository.GetAllData().Count();
        }

        public int GetTotalProductPurchased()
        {
            return (int)context.ProductRepository.GetAllData().Sum(x => x.PurchasedCount);
        }

        public IEnumerable<Product> GetProductListStocking()
        {
            return context.ProductRepository.GetAllData(x => x.Quantity > 0 && x.IsActive == true);
        }

        public IEnumerable<Product> GetProductListSold(DateTime from, DateTime to)
        {
            IEnumerable<OrderDetail> orderDetails = context.OrderDetailRepository.GetAllData(x => DbFunctions.TruncateTime(x.Order.DateOrder) >= from.Date && DbFunctions.TruncateTime(x.Order.DateOrder) <= to.Date);

            List<int> ProductIDs = new List<int>();
            foreach (var item in orderDetails)
            {
                ProductIDs.Add(item.ProductID);
            }
            if (ProductIDs.Count() > 0)
            {
                return context.ProductRepository.GetAllData(x => x.PurchasedCount > 0 && ProductIDs.Contains(x.ID)).OrderByDescending(x => x.PurchasedCount);
            }
            return null;
        }
        public Product GetByName(string Name)
        {
            Product product = context.ProductRepository.GetAllData().FirstOrDefault(x => x.Name == Name);
            return product;
        }
        public bool CheckName(string Name)
        {
            var check = context.ProductRepository.GetAllData(x => x.Name == Name && x.IsActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<Product> GetProductListDiscount()
        {
            return context.ProductRepository.GetAllData(x => x.Discount > 0 && x.IsActive == true).Take(3);
        }
    }
}