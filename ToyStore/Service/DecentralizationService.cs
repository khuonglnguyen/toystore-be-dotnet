using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface IDecentralizationService
    {
        IEnumerable<Decentralization> GetDecentralizationByUserTypeID(int ID);
        void RemoveRange(IEnumerable<Decentralization> decentralizations);
        void Add(Decentralization decentralization);
    }
    public class DecentralizationService: IDecentralizationService
    {
        private readonly UnitOfWork context;
        public DecentralizationService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void Add(Decentralization decentralization)
        {
            context.DecentralizationRepository.Insert(decentralization);
        }

        public IEnumerable<Decentralization> GetDecentralizationByUserTypeID(int ID)
        {
            return context.DecentralizationRepository.GetAllData(x => x.UserTypeID == ID);
        }

        public void RemoveRange(IEnumerable<Decentralization> decentralizations)
        {
            foreach (var item in decentralizations)
            {
                context.DecentralizationRepository.Remove(item);
            }
        }
    }
}