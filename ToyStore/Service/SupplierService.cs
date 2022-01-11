using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ToyStore.Data;
using ToyStore.Extensions;
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
        Supplier GetByPhoneNumber(string PhoneNumber);
        Supplier GetByName(string Name);
        Supplier GetByEmail(string Email);
        bool CheckPhoneNumber(string PhoneNumber);
        bool CheckName(string Name);
        bool CheckEmail(string Email);
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
            supplier.TotalAmount = 0;
            supplier.SEOKeyword = StringHelper.UrlFriendly(supplier.Name);
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
        public bool CheckPhoneNumber(string PhoneNumber)
        {
            var check = context.SupplierRepository.GetAllData(x => x.Phone == PhoneNumber && x.IsActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool CheckName(string Name)
        {
            var check = context.SupplierRepository.GetAllData(x => x.Name == Name && x.IsActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool CheckEmail(string Email)
        {
            var check = context.SupplierRepository.GetAllData(x => x.Email == Email && x.IsActive == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public Supplier GetByPhoneNumber(string PhoneNumber)
        {
            Supplier supplier = context.SupplierRepository.GetAllData().FirstOrDefault(x => x.Phone == PhoneNumber);
            return supplier;
        }

        public Supplier GetByName(string Name)
        {
            Supplier supplier = context.SupplierRepository.GetAllData().FirstOrDefault(x => x.Name == Name);
            return supplier;
        }

        public Supplier GetByEmail(string Email)
        {
            Supplier supplier = context.SupplierRepository.GetAllData().FirstOrDefault(x => x.Email == Email);
            return supplier;
        }
    }
}