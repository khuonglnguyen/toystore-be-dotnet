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
        DiscountCodeDetail GetByID(int ID);
        IEnumerable<DiscountCodeDetail> GetDiscountCodeDetailList();
        IEnumerable<MemberDiscountCode> GetDiscountCodeDetailListByMember(int MemberID);
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

        public int GetDiscountByCode(string Code)
        {
            return (int)context.DiscountCodeDetailRepository.GetAllData().SingleOrDefault(x => x.Code.Contains(Code)).DiscountCode.NumberDiscount;
        }

        public IEnumerable<DiscountCodeDetail> GetDiscountCodeDetailList()
        {
            return context.DiscountCodeDetailRepository.GetAllData(x=>x.IsUsed==false);
        }

        public IEnumerable<MemberDiscountCode> GetDiscountCodeDetailListByMember(int MemberID)
        {
            return context.MemberDiscountCodeRepository.GetAllData(x => x.MemberID == MemberID && x.DiscountCodeDetail.IsUsed==false);
        }

        public void Used(string code)
        {
            DiscountCodeDetail discountCodeDetail = context.DiscountCodeDetailRepository.GetAllData().SingleOrDefault(x => x.Code == code);
            discountCodeDetail.IsUsed = true;
            context.DiscountCodeDetailRepository.Update(discountCodeDetail);
        }
    }
}