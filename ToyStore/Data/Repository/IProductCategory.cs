using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyStore.Models;

namespace ToyStore.Data.Repository
{
    interface IProductCategory
    {
        void Insert(ProductCategory productCategory); // C

        IEnumerable<ProductCategory> GetAll(); // R

        ProductCategory GetByID(int ID); // R

        void Update(ProductCategory productCategory); //U

        void Delete(int ID); //D

        void Save();
    }
}
