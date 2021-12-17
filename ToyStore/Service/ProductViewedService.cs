using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IProductViewedService
    {
        void AddProductViewByUser(int productID, int userID);
        void Delete(int userID);
    }
    public class ProductViewedService : IProductViewedService
    {
        private readonly UnitOfWork context;
        public ProductViewedService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public void AddProductViewByUser(int productID, int userID)
        {
            try
            {
                ProductViewed productVieweds = context.ProductViewedRepository.GetAllData().Single(x => x.ProductID == productID && x.UserID == userID);
                if (productVieweds != null)
                {
                    productVieweds.Date = DateTime.Now;
                    context.ProductViewedRepository.Update(productVieweds);
                }
            }
            catch (Exception)
            {
                ProductViewed productViewed = new ProductViewed();
                productViewed.ProductID = productID;
                productViewed.UserID = userID;
                productViewed.Date = DateTime.Now;
                context.ProductViewedRepository.Insert(productViewed);
            }
        }

        public void Delete(int userID)
        {
            IEnumerable<ProductViewed> productViewed = context.ProductViewedRepository.GetAllData(x => x.UserID == userID);
            foreach (var item in productViewed)
            {
                context.ProductViewedRepository.Remove(item);
            }
        }
    }
}