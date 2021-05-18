using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IMemberDiscountCodeService
    {
        void GiftForNewMember(int MemberID);
    }
    public class MemberDiscountCodeService : IMemberDiscountCodeService
    {
        private readonly UnitOfWork context;
        public MemberDiscountCodeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void GiftForNewMember(int MemberID)
        {
            MemberDiscountCode member = new MemberDiscountCode();
            string code = context.DiscountCodeRepository.GetAllData(x => x.NumberDiscount == 10).First().DiscountCodeDetails.First(x => x.IsUsed == false).Code;
            member.DiscountCodeDetailID = context.DiscountCodeDetailRepository.GetAllData(x => x.Code == code).First().ID;
            member.MemberID = MemberID;
            context.MemberDiscountCodeRepository.Insert(member);
        }
    }
}