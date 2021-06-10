using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IDiscountCodeService
    {
        IEnumerable<DiscountCode> GetDiscountCodeList();
        DiscountCode GetByID(int ID);
        void Block(int ID);
        void Active(int ID);
        void AddDiscountCode(DiscountCode discountCode, int quantity);
        bool CheckCode(string Code);
    }
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly UnitOfWork context;
        public DiscountCodeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void Active(int ID)
        {
            DiscountCode discount = context.DiscountCodeRepository.GetDataByID(ID);
            discount.IsActive = true;
            context.DiscountCodeRepository.Update(discount);
        }

        public void AddDiscountCode(DiscountCode discountCode, int quantity)
        {
            discountCode.IsActive = true;
            context.DiscountCodeRepository.Insert(discountCode);
            DiscountCodeDetail discountCodeDetail = new DiscountCodeDetail();
            discountCodeDetail.DiscountCodeID = discountCode.ID;
            discountCodeDetail.IsUsed = false;
            Random random = new Random();
            for (int i = 0; i < quantity; i++)
            {
                lock (discountCodeDetail)
                { // synchronize
                    string strString = "abcdefghijklmnopqrstuvwxyz0123456789";
                    int randomCharIndex = 0;
                    char randomChar;
                    string captcha = "";
                    for (int j = 0; j < 5; j++)
                    {
                        randomCharIndex = random.Next(0, strString.Length);
                        randomChar = strString[randomCharIndex];
                        captcha += Convert.ToString(randomChar);
                    }
                    discountCodeDetail.Code = captcha;
                    context.DiscountCodeDetailRepository.Insert(discountCodeDetail);
                }
            }
        }
        public void Block(int ID)
        {
            DiscountCode discount = context.DiscountCodeRepository.GetDataByID(ID);
            discount.IsActive = false;
            context.DiscountCodeRepository.Update(discount);
        }

        public bool CheckCode(string Code)
        {
            DiscountCodeDetail discountCodeDetail = context.DiscountCodeDetailRepository.GetAllData().FirstOrDefault(x => x.Code == Code);
            if (discountCodeDetail != null)
            {
                return true;
            }
            return false;
        }

        public DiscountCode GetByID(int ID)
        {
            return context.DiscountCodeRepository.GetDataByID(ID);
        }

        public IEnumerable<DiscountCode> GetDiscountCodeList()
        {
            return context.DiscountCodeRepository.GetAllData();
        }
    }
}