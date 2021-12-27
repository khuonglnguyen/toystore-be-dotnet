using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IUsersSpinService
    {
        int GetNumberOfSpinsByUserID(int UserID);
        bool AddOnceSpin(int UserID);
        bool SubOnceSpin(int UserID);
        bool Add(UsersSpin users);
    }
    public class UsersSpinService : IUsersSpinService
    {
        private readonly UnitOfWork context;
        public UsersSpinService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public bool Add(UsersSpin users)
        {
            try
            {
                this.context.UsersSpinRepository.Insert(users);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddOnceSpin(int UserID)
        {
            try
            {
                UsersSpin users = this.context.UsersSpinRepository.GetAllData(x => x.UserID == UserID).FirstOrDefault();
                users.NumberOfSpins += 1;
                this.context.UsersSpinRepository.Update(users);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SubOnceSpin(int UserID)
        {
            try
            {
                UsersSpin users = this.context.UsersSpinRepository.GetAllData(x => x.UserID == UserID).FirstOrDefault();
                users.NumberOfSpins -= 1;
                this.context.UsersSpinRepository.Update(users);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetNumberOfSpinsByUserID(int UserID)
        {
            return this.context.UsersSpinRepository.GetAllData(x=>x.UserID == UserID).FirstOrDefault().NumberOfSpins.Value;
        }
    }
}