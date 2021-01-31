using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToyStore.Models;

namespace ToyStore.Data.Repository
{
    interface IGenericRepository<T> where T : class
    {
        void Insert(T Model); // C

        IEnumerable<T> GetAllData(); // R

        T GetDataByID(int ID); // R

        void Update(T Model); //U

        void Delete(T Model); //D

        void Save();
    }
}
