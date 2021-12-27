using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IDiscountCodeDetailService
    {
        int GetDiscountByCode(string Code);
        int GetDiscountByCodeInput(string Code);
        string GetCodeFirstByNumberDiscount(int NumberDiscount);
        DiscountCodeDetail GetByID(int ID);
        IEnumerable<DiscountCodeDetail> GetDiscountCodeDetailList();
        IEnumerable<UserDiscountCode> GetDiscountCodeDetailListByUser(int UserID);
        void Used(string code);
    }
    public class DiscountCodeDetailService : IDiscountCodeDetailService
    {
        private readonly UnitOfWork context;
        public DiscountCodeDetailService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public DiscountCodeDetail GetByID(int ID)
        {
            return context.DiscountCodeDetailRepository.GetDataByID(ID);
        }

        public string GetCodeFirstByNumberDiscount(int NumberDiscount)
        {
            return context.DiscountCodeDetailRepository.GetAllData(x => x.DiscountCode.NumberDiscount == NumberDiscount && x.IsUsed == false && x.IsOwned == false).FirstOrDefault().Code;
        }

        public int GetDiscountByCode(string Code)
        {
            try
            {
                return (int)context.DiscountCodeDetailRepository.GetAllData().SingleOrDefault(x => x.Code.Contains(Code)).DiscountCode.NumberDiscount;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int GetDiscountByCodeInput(string Code)
        {
            try
            {
                return (int)context.DiscountCodeDetailRepository.GetAllData(x => x.IsUsed == false && x.IsOwned == false).SingleOrDefault(x => x.Code.Contains(Code)).DiscountCode.NumberDiscount;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public IEnumerable<DiscountCodeDetail> GetDiscountCodeDetailList()
        {
            return context.DiscountCodeDetailRepository.GetAllData(x => x.IsUsed == false);
        }

        public IEnumerable<UserDiscountCode> GetDiscountCodeDetailListByUser(int UserID)
        {
            return context.UserDiscountCodeRepository.GetAllData(x => x.UserID == UserID && x.DiscountCodeDetail.IsUsed == false && x.DiscountCodeDetail.DiscountCode.ExpirationDate.Value >= DateTime.Now);
        }

        public void Used(string code)
        {
            DiscountCodeDetail discountCodeDetail = context.DiscountCodeDetailRepository.GetAllData().SingleOrDefault(x => x.Code == code);
            discountCodeDetail.IsUsed = true;
            context.DiscountCodeDetailRepository.Update(discountCodeDetail);
        }
    }
}