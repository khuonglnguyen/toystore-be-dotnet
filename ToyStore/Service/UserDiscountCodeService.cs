using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IUserDiscountCodeService
    {
        void GiftForNewUser(int UserID);
        bool GiftSpin(int UserID, string Code);
    }
    public class UserDiscountCodeService : IUserDiscountCodeService
    {
        private readonly UnitOfWork context;
        public UserDiscountCodeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void GiftForNewUser(int UserID)
        {
            UserDiscountCode user = new UserDiscountCode();
            string code = context.DiscountCodeRepository.GetAllData(x => x.NumberDiscount == 10).First().DiscountCodeDetails.First(x => x.IsUsed == false).Code;
            user.DiscountCodeDetailID = context.DiscountCodeDetailRepository.GetAllData(x => x.Code == code).First().ID;
            user.UserID = UserID;
            context.UserDiscountCodeRepository.Insert(user);

            //Update IsOwned
            DiscountCodeDetail discount = context.DiscountCodeDetailRepository.GetAllData(x => x.Code == code).First();
            discount.IsOwned = true;
            context.DiscountCodeDetailRepository.Update(discount);
        }

        public bool GiftSpin(int UserID, string Code)
        {
            try
            {
                UserDiscountCode user = new UserDiscountCode();
                user.UserID = UserID;
                user.DiscountCodeDetailID = context.DiscountCodeDetailRepository.GetAllData(x => x.Code == Code).FirstOrDefault().ID;
                context.UserDiscountCodeRepository.Insert(user);

                DiscountCodeDetail discountCodeDetail = context.DiscountCodeDetailRepository.GetAllData(x => x.Code == Code).FirstOrDefault();
                discountCodeDetail.IsOwned = true;
                context.DiscountCodeDetailRepository.Update(discountCodeDetail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}