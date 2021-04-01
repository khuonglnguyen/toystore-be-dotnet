using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IImportCouponDetailService
    {
        ImportCouponDetail AddImportCouponDetail(ImportCouponDetail import);
        IEnumerable<ImportCouponDetail> GetByImportCouponID(int ID);
    }
    public class ImportCouponDetailService : IImportCouponDetailService
    {
        private readonly UnitOfWork context;
        public ImportCouponDetailService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public ImportCouponDetail AddImportCouponDetail(ImportCouponDetail import)
        {
            context.ImportCouponDetailRepository.Insert(import);
            return import;
        }

        public IEnumerable<ImportCouponDetail> GetByImportCouponID(int ID)
        {
            return context.ImportCouponDetailRepository.GetAllData().Where(x => x.ImportCouponID == ID);
        }
    }
}