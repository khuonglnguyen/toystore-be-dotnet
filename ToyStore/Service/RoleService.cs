using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRoleList();
        void AddRole(Role role);
        void UpdateRole(Role role);
        void BlockRole(Role role);
        void ActiveRole(Role role);
        Role GetRoleByID(int ID);
    }
    public class RoleService : IRoleService
    {
        private readonly UnitOfWork context;
        public RoleService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void ActiveRole(Role role)
        {
            role.IsActive = true;
            context.RoleRepository.Update(role);
        }

        public void AddRole(Role role)
        {
            role.IsActive = true;
            context.RoleRepository.Insert(role);
        }

        public void BlockRole(Role role)
        {
            role.IsActive = false;
            context.RoleRepository.Update(role);
        }

        public Role GetRoleByID(int ID)
        {
            return context.RoleRepository.GetDataByID(ID);
        }

        public IEnumerable<Role> GetRoleList()
        {
            return context.RoleRepository.GetAllData();
        }

        public void UpdateRole(Role role)
        {
            role.IsActive = true;
            context.RoleRepository.Update(role);
        }
    }
}