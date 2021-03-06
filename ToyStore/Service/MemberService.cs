using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IMemberService
    {
        Member AddMember(Member member);
        IEnumerable<Member> GetMemberList();
        Member CheckLogin(string username, string password);
        Member GetByID(int ID);
        void UpdateMember(Member member);
        void DeleteMember(Member member);
        void Save();
    }
    public class MemberService : IMemberService
    {
        private readonly UnitOfWork context;
        public MemberService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Member AddMember(Member member)
        {
            member.MemberCategoryID = 1;
            this.context.MemberRepository.Insert(member);
            return member;
        }

        public Member CheckLogin(string username, string password)
        {
            Member member = this.context.MemberRepository.GetAllData().SingleOrDefault(x => x.Username == username && x.Password == password);
            if (member == null)
            {
                member = this.context.MemberRepository.GetAllData().SingleOrDefault(x => x.Email == username && x.Password == password);
            }
            return member;
        }

        public void DeleteMember(Member member)
        {

        }

        public Member GetByID(int ID)
        {
            return this.context.MemberRepository.GetDataByID(ID);
        }

        public IEnumerable<Member> GetMemberList()
        {
            IEnumerable<Member> listMember = this.context.MemberRepository.GetAllData();
            return listMember;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateMember(Member member)
        {
            this.context.MemberRepository.Update(member);
        }
    }
}