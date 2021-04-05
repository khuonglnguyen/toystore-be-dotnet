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
        void AddProductViewByMember(int productID, int memberID);
        void Delete(int memberID);
    }
    public class ProductViewedService : IProductViewedService
    {
        private readonly UnitOfWork context;
        public ProductViewedService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public void AddProductViewByMember(int productID, int memberID)
        {
            try
            {
                ProductViewed productVieweds = context.ProductViewedRepository.GetAllData().Single(x => x.ProductID == productID && x.MemberID == memberID);
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
                productViewed.MemberID = memberID;
                productViewed.Date = DateTime.Now;
                context.ProductViewedRepository.Insert(productViewed);
            }
        }

        public void Delete(int memberID)
        {
            IEnumerable<ProductViewed> productViewed = context.ProductViewedRepository.GetAllData(x => x.MemberID == memberID);
            foreach (var item in productViewed)
            {
                context.ProductViewedRepository.Remove(item);
            }
        }
    }
}