using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IImportCouponService
    {
        ImportCoupon GetByID(int ID);
        ImportCoupon AddImportCoupon(ImportCoupon importCoupon);
        IEnumerable<ImportCoupon> GetImportCoupon();
        void Delete(int ID);
        void Rehibilitate(int ID);
    }
    public class ImportCouponService : IImportCouponService
    {
        private readonly UnitOfWork context;
        public ImportCouponService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public ImportCoupon AddImportCoupon(ImportCoupon importCoupon)
        {
            context.ImportCouponRepository.Insert(importCoupon);
            return importCoupon;
        }

        public void Delete(int ID)
        {
            ImportCoupon importCoupon = context.ImportCouponRepository.GetDataByID(ID);
            importCoupon.IsDelete = true;
            context.ImportCouponRepository.Update(importCoupon);
        }

        public ImportCoupon GetByID(int ID)
        {
            return context.ImportCouponRepository.GetDataByID(ID);
        }

        public IEnumerable<ImportCoupon> GetImportCoupon()
        {
            return context.ImportCouponRepository.GetAllData();
        }

        public void Rehibilitate(int ID)
        {
            ImportCoupon importCoupon = context.ImportCouponRepository.GetDataByID(ID);
            importCoupon.IsDelete = false;
            context.ImportCouponRepository.Update(importCoupon);
        }
    }
}