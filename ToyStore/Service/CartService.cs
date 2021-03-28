using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface ICartService
    {
        void AddCartIntoMember(ItemCart itemCart, int MemberID);
        bool CheckCartMember(int MemberID);
        void UpdateQuantityCartMember(int Quantity, int ProductID);
        IEnumerable<Cart> GetCart(int MemberID);
    }
    public class CartService : ICartService
    {
        private readonly UnitOfWork context;
        public CartService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public void AddCartIntoMember(ItemCart itemCart, int MemberID)
        {
            Cart cart = new Cart();
            cart.ProductID = itemCart.ID;
            cart.MemberID = MemberID;
            cart.Price = itemCart.Price;
            cart.Total = itemCart.Total;
            cart.Image = itemCart.Image;
            cart.Name = itemCart.Name;
            cart.Quantity = itemCart.Quantity;
            context.CartRepository.Insert(cart);
        }

        public bool CheckCartMember(int MemberID)
        {
            if (context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID).Count() > 0)
                return true;
            return false;
        }

        public IEnumerable<Cart> GetCart(int MemberID)
        {
            return context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID);
        }

        public void UpdateQuantityCartMember(int Quantity, int ProductID)
        {
            Cart cartUpdate = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID);
            cartUpdate.Quantity = Quantity;
            context.CartRepository.Update(cartUpdate);
        }
    }
}