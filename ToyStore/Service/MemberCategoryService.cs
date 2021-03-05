using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IMemberCategoryService
    {
        MemberCategory AddMemberCategory(MemberCategory memberCategory);
        IEnumerable<MemberCategory> GetMemberCategoryList();
        IEnumerable<MemberCategory> GetMemberCategoryList(string keyWord);
        IEnumerable<MemberCategory> GetMemberCategoryListName(string keyword);
        MemberCategory GetByID(int ID);
        void UpdateMemberCategory(MemberCategory memberCategory);
        void DeleteMemberCategory(MemberCategory memberCategory);
        void Save();
    }
    public class MemberCategoryService : IMemberCategoryService
    {
        private readonly UnitOfWork context;
        public MemberCategoryService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public MemberCategory AddMemberCategory(MemberCategory memberCategory)
        {
            this.context.MemberCategoryRepository.Insert(memberCategory);
            return memberCategory;
        }

        public void DeleteMemberCategory(MemberCategory memberCategory)
        {

        }

        public MemberCategory GetByID(int ID)
        {
            return this.context.MemberCategoryRepository.GetDataByID(ID);
        }

        public IEnumerable<MemberCategory> GetMemberCategoryList()
        {
            IEnumerable<MemberCategory> listMemberCategory = this.context.MemberCategoryRepository.GetAllData();
            return listMemberCategory;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateMemberCategory(MemberCategory memberCategory)
        {
            this.context.MemberCategoryRepository.Update(memberCategory);
        }

        public IEnumerable<MemberCategory> GetMemberCategoryList(string keyWord)
        {
            IEnumerable<MemberCategory> listMemberCategory = this.context.MemberCategoryRepository.GetAllData(x => x.Name.Contains(keyWord));
            return listMemberCategory;
        }

        public IEnumerable<MemberCategory> GetMemberCategoryListName(string keyword)
        {
            IEnumerable<MemberCategory> listMemberCategoryName = this.context.MemberCategoryRepository.GetAllData(x => x.Name.Contains(keyword));
            return listMemberCategoryName;
        }
    }
}