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
    }
    public class DiscountCodeService: IDiscountCodeService
    {
        private readonly UnitOfWork context;
        public DiscountCodeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public IEnumerable<DiscountCode> GetDiscountCodeList()
        {
            return context.DiscountCodeRepository.GetAllData();
        }
    }
}