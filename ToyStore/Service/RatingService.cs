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

        public int GetRating(int ProductID)
        {
            IEnumerable<Rating> ratings = context.RatingRepository.GetAllData(x => x.ProductID == ProductID);
            List<int> list = ratings.Select(x=>x.Star).ToList();
            int sum = 0;
            foreach (int item in list)
            {
                sum += item;
            }
            return sum/list.Count;
        }
    }
}