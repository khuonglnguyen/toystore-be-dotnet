using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IMemberTypeService
    {
        MemberType AddMemberType(MemberType memberType);
        IEnumerable<MemberType> GetMemberTypeList();
        IEnumerable<MemberType> GetMemberTypeList(string keyWord);
        IEnumerable<MemberType> GetMemberTypeListName(string keyword);
        MemberType GetByID(int ID);
        void UpdateMemberType(MemberType memberType);
        void DeleteMemberType(MemberType memberType);
        void Save();
    }
    public class MemberTypeService : IMemberTypeService
    {
        private readonly UnitOfWork context;
        public MemberTypeService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public MemberType AddMemberType(MemberType memberType)
        {
            this.context.MemberTypeRepository.Insert(memberType);
            return memberType;
        }

        public void DeleteMemberType(MemberType memberType)
        {

        }

        public MemberType GetByID(int ID)
        {
            return this.context.MemberTypeRepository.GetDataByID(ID);
        }

        public IEnumerable<MemberType> GetMemberTypeList()
        {
            IEnumerable<MemberType> listMemberType = this.context.MemberTypeRepository.GetAllData();
            return listMemberType;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateMemberType(MemberType memberType)
        {
            this.context.MemberTypeRepository.Update(memberType);
        }

        public IEnumerable<MemberType> GetMemberTypeList(string keyWord)
        {
            IEnumerable<MemberType> listMemberType = this.context.MemberTypeRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listMemberType;
        }

        public IEnumerable<MemberType> GetMemberTypeListName(string keyword)
        {
            IEnumerable<MemberType> listMemberTypeName = this.context.MemberTypeRepository.GetAllData(x => x.Name.Contains(keyword));
            return listMemberTypeName;
        }
    }
}