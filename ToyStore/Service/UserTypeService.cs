using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IUserTypeService
    {
        UserType GetUserTypeByID(int ID);
        IEnumerable<UserType> GetListUserType();
        UserType Add(UserType userType);
        IEnumerable<UserType> GetUserTypeList();
        IEnumerable<UserType> GetUserTypeList(string keyWord);
        IEnumerable<UserType> GetUserTypeListName(string keyword);
        UserType GetByID(int ID);
        void Update(UserType userType);
        void Delete(UserType userType);
        void Save();
    }
    public class UserTypeService : IUserTypeService
    {
        private readonly UnitOfWork context;
        public UserTypeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public UserType GetUserTypeByID(int ID)
        {
            return context.UserTypeRepository.GetDataByID(ID);
        }

        public IEnumerable<UserType> GetListUserType()
        {
            return context.UserTypeRepository.GetAllData();
        }

        public UserType Add(UserType userType)
        {
            this.context.UserTypeRepository.Insert(userType);
            return userType;
        }

        public IEnumerable<UserType> GetUserTypeList()
        {
            IEnumerable<UserType> listUserType = this.context.UserTypeRepository.GetAllData();
            return listUserType;
        }

        public IEnumerable<UserType> GetUserTypeList(string keyWord)
        {
            IEnumerable<UserType> listUserType = this.context.UserTypeRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listUserType;
        }

        public IEnumerable<UserType> GetUserTypeListName(string keyword)
        {
            IEnumerable<UserType> listUserTypeName = this.context.UserTypeRepository.GetAllData(x => x.Name.Contains(keyword));
            return listUserTypeName;
        }

        public UserType GetByID(int ID)
        {
            return this.context.UserTypeRepository.GetDataByID(ID);
        }

        public void Update(UserType userType)
        {
            this.context.UserTypeRepository.Update(userType);
        }

        public void Delete(UserType userType)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}