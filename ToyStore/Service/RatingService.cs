using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IRatingService
    {
        void AddRating(Rating rating);
        int GetRating(int ProductID);
        IEnumerable<Rating> GetListRating(int ProductID);
        IEnumerable<Rating> GetListAllRating();
    }
    public class RatingService : IRatingService
    {
        private readonly UnitOfWork context;
        public RatingService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public void AddRating(Rating rating)
        {
            context.RatingRepository.Insert(rating);
        }

        public IEnumerable<Rating> GetListAllRating()
        {
            return context.RatingRepository.GetAllData();
        }

        public IEnumerable<Rating> GetListRating(int ProductID)
        {
            return context.RatingRepository.GetAllData(x => x.ProductID == ProductID);
        }

        public int GetRating(int ProductID)
        {
            IEnumerable<Rating> ratings = context.RatingRepository.GetAllData(x => x.ProductID == ProductID);
            List<int> list = ratings.Select(x => x.Star).ToList();
            int sum = 0;
            foreach (int item in list)
            {
                sum += item;
            }
            if (sum > 0)
                return sum / list.Count;
            else
                return 0;
        }
    }
}