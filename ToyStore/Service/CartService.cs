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
        void AddCartIntoMember(ItemCart cart);
        bool CheckCartMember(int MemberID);
        void UpdateQuantityCartMember(int Quantity, int ProductID, int MemberID);
        void AddQuantityProductCartMember(int ProductID, int MemberID);
        void RemoveCart(int ProductID, int MemberID);
        void AddCart(ItemCart cart);
        List<ItemCart> GetCart(int MemberID);
        ItemCart GetCartByID(int ID);
        bool CheckProductInCart(int ProductID, int MemberID);
        void RemoveCartDeleteAccount(int MemberID);
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

        public void AddCartIntoMember(ItemCart cart)
        {
            context.CartRepository.Insert(cart);
        }

        public void AddQuantityProductCartMember(int ProductID, int MemberID)
        {
            ItemCart cartUpdate = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.MemberID == MemberID);
            cartUpdate.Quantity += 1;
            context.CartRepository.Update(cartUpdate);
        }

        public bool CheckCartMember(int MemberID)
        {
            if (context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID).Count() > 0)
                return true;
            return false;
        }

        public bool CheckProductInCart(int ProductID, int MemberID)
        {
            if (context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID && x.ProductID == ProductID).Count() > 0)
                return true;
            return false;
        }

        public List<ItemCart> GetCart(int MemberID)
        {
            return context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID).ToList();
        }

        public ItemCart GetCartByID(int ID)
        {
            return context.CartRepository.GetDataByID(ID);
        }

        public void RemoveCart(int ProductID, int MemberID)
        {
            ItemCart cart = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.MemberID == MemberID);
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

        public void UpdateQuantityCartMember(int Quantity, int ProductID, int MemberID)
        {
            ItemCart cartUpdate = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.MemberID == MemberID);
            cartUpdate.Quantity = Quantity;
            cartUpdate.Total = cartUpdate.Quantity * cartUpdate.Product.PromotionPrice;
            context.CartRepository.Update(cartUpdate);
        }
    }
}