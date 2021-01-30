using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyStore.Models;

namespace ToyStore.Data.Repository
{
    interface IProduct
    {
        void Insert(Product product); // C

        IEnumerable<Product> GetAll(); // R

        Product GetByID(int ID); // R

        void Update(Product product); //U

        void Delete(int ID); //D

        void Save();
    }
}
