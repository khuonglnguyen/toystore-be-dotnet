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
        void AddCartIntoUser(ItemCart cart);
        bool CheckCartUser(int UserID);
        void UpdateQuantityCartUser(int Quantity, int ProductID, int UserID);
        void AddQuantityProductCartUser(int ProductID, int UserID);
        void RemoveCart(int ProductID, int UserID);
        void AddCart(ItemCart cart);
        List<ItemCart> GetCart(int UserID);
        ItemCart GetCartByID(int ID);
        bool CheckProductInCart(int ProductID, int UserID);
        void RemoveCartDeleteAccount(int UserID);
    }
    public class CartService : ICartService
    {
        private readonly UnitOfWork context;
        public CartService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void AddCart(ItemCart cart)
        {
            context.CartRepository.Insert(cart);
        }

        public void AddCartIntoUser(ItemCart cart)
        {
            context.CartRepository.Insert(cart);
        }

        public void AddQuantityProductCartUser(int ProductID, int UserID)
        {
            ItemCart cartUpdate = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.UserID == UserID);
            cartUpdate.Quantity += 1;
            cartUpdate.Total = cartUpdate.Quantity * cartUpdate.Product.Price;
            context.CartRepository.Update(cartUpdate);
        }

        public bool CheckCartUser(int UserID)
        {
            if (context.CartRepository.GetAllData().Where(x => x.UserID == UserID).Count() > 0)
                return true;
            return false;
        }

        public bool CheckProductInCart(int ProductID, int UserID)
        {
            if (context.CartRepository.GetAllData().Where(x => x.UserID == UserID && x.ProductID == ProductID).Count() > 0)
                return true;
            return false;
        }

        public List<ItemCart> GetCart(int UserID)
        {
            return context.CartRepository.GetAllData().Where(x => x.UserID == UserID).ToList();
        }

        public ItemCart GetCartByID(int ID)
        {
            return context.CartRepository.GetDataByID(ID);
        }

        public void RemoveCart(int ProductID, int MemberID)
        {
            ItemCart cart = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.UserID == MemberID);
            context.CartRepository.Remove(cart);
        }

        public void RemoveCartDeleteAccount(int MemberID)
        {
            List<ItemCart> cart = GetCart(MemberID);
            if(cart!=null)
            {
                foreach (ItemCart item in cart)
                {
                    context.CartRepository.Remove(item);
                }
            }    
        }

        public void UpdateQuantityCartUser(int Quantity, int ProductID, int UserID)
        {
            ItemCart cartUpdate = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.UserID == UserID);
            cartUpdate.Quantity = Quantity;
            cartUpdate.Total = cartUpdate.Quantity * cartUpdate.Product.PromotionPrice;
            context.CartRepository.Update(cartUpdate);
        }
    }
}