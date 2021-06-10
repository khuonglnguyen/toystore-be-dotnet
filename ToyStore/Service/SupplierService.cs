using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface ISupplierService
    {
        Supplier AddSupplier(Supplier supplier);
        IEnumerable<Supplier> GetSupplierList();
        IEnumerable<Supplier> GetSupplierList(string keyWord);
        List<string> GetSupplierListName(string keyword);
        Supplier GetByID(int ID);
        void UpdateSupplier(Supplier supplier);
        void DeleteSupplier(Supplier supplier);
        void MultiDeleteSupplier(string[] IDs);
        void Block(int ID);
        void Active(int ID);
        void Save();
    }
    public class SupplierService : ISupplierService
    {
        private readonly UnitOfWork context;
        public SupplierService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Supplier AddSupplier(Supplier supplier)
        {
            supplier.LastUpdatedDate = DateTime.Now;
            this.context.SupplierRepository.Insert(supplier);
            return supplier;
        }

        public void DeleteSupplier(Supplier supplier)
        {
            supplier.IsActive = false;
            this.context.SupplierRepository.Delete(supplier);
        }
        public void MultiDeleteSupplier(string[] IDs)
        {
            foreach (var id in IDs)
            {
                Supplier supplier = GetByID(int.Parse(id));
                supplier.IsActive = false;
                UpdateSupplier(supplier);
            }
        }

        public Supplier GetByID(int ID)
        {
            return this.context.SupplierRepository.GetDataByID(ID);
        }

        public IEnumerable<Supplier> GetSupplierList()
        {
            IEnumerable<Supplier> listSupplier = this.context.SupplierRepository.GetAllData().OrderByDescending(x => x.TotalAmount);
            return listSupplier;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            supplier.LastUpdatedDate = DateTime.Now;
            this.context.SupplierRepository.Update(supplier);
        }

        public IEnumerable<Supplier> GetSupplierList(string keyWord)
        {
            IEnumerable<Supplier> listSupplier = this.context.SupplierRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listSupplier;
        }

        public void Block(int ID)
        {
            Supplier supplier = context.SupplierRepository.GetDataByID(ID);
            supplier.IsActive = false;
            context.SupplierRepository.Update(supplier);
        }

        public void Active(int ID)
        {
            Supplier supplier = context.SupplierRepository.GetDataByID(ID);
            supplier.IsActive = true;
            context.SupplierRepository.Update(supplier);
        }

        public List<string> GetSupplierListName(string keyword)
        {
            IEnumerable<Supplier> listSupplierName = this.context.SupplierRepository.GetAllData(x => x.Name.Contains(keyword) && x.IsActive == true);
            List<string> names = new List<string>();
            foreach (var item in listSupplierName)
            {
                names.Add(item.Name);
            }
            return names;
        }
    }
}