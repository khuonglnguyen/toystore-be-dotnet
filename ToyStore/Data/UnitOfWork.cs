using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data.Repository;
using ToyStore.Models;

namespace ToyStore.Data
{
    public class UnitOfWork : IDisposable
    {
        private ToyStore2021Entities DbContext = new ToyStore2021Entities();
        private GenericRepository<Product> productRepository;
        private GenericRepository<ProductCategory> productCategoryRepository;
        private GenericRepository<Supplier> supplierRepository;
        private GenericRepository<Producer> producerRepository;
        private GenericRepository<Age> ageRepository;
        private GenericRepository<ProductCategoryParent> productCategoryParentRepository;
        private GenericRepository<Gender> genderParentRepository;
        private GenericRepository<MemberCategory> memberCategoryRepository;
        private GenericRepository<Member> memberRepository;
        private GenericRepository<Customer> customerRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<OrderDetail> orderDetailRepository;
        private GenericRepository<Comment> commentRepository;
        private GenericRepository<QA> qARepository;
        private GenericRepository<Emloyee> emloyeeRepository;
        private GenericRepository<EmloyeeType> emloyeeTypeRepository;
        private GenericRepository<AccessTimesCount> accessTimesCountRepository;
        private GenericRepository<Cart> cartRepository;
        private GenericRepository<ImportCoupon> importCouponRepository;
        private GenericRepository<ImportCouponDetail> importCouponDetailRepository;
        private GenericRepository<ProductViewed> productViewedRepository;
        private GenericRepository<Rating> ratingRepository;
        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<Product>(DbContext);
                }
                return productRepository;
            }
        }
        public GenericRepository<ProductCategory> ProductCategoryRepository
        {
            get
            {
                if (this.productCategoryRepository == null)
                {
                    this.productCategoryRepository = new GenericRepository<ProductCategory>(DbContext);
                }
                return productCategoryRepository;
            }
        }
        public GenericRepository<Supplier> SupplierRepository
        {
            get
            {
                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new GenericRepository<Supplier>(DbContext);
                }
                return supplierRepository;
            }
        }
        public GenericRepository<Producer> ProducerRepository
        {
            get
            {
                if (this.producerRepository == null)
                {
                    this.producerRepository = new GenericRepository<Producer>(DbContext);
                }
                return producerRepository;
            }
        }
        public GenericRepository<Age> AgeRepository
        {
            get
            {
                if (this.ageRepository == null)
                {
                    this.ageRepository = new GenericRepository<Age>(DbContext);
                }
                return ageRepository;
            }
        }
        public GenericRepository<ProductCategoryParent> ProductCategoryParentRepository
        {
            get
            {
                if (this.productCategoryParentRepository == null)
                {
                    this.productCategoryParentRepository = new GenericRepository<ProductCategoryParent>(DbContext);
                }
                return productCategoryParentRepository;
            }
        }
        public GenericRepository<Gender> GenderRepository
        {
            get
            {
                if (this.genderParentRepository == null)
                {
                    this.genderParentRepository = new GenericRepository<Gender>(DbContext);
                }
                return genderParentRepository;
            }
        }
        public GenericRepository<MemberCategory> MemberCategoryRepository
        {
            get
            {
                if (this.memberCategoryRepository == null)
                {
                    this.memberCategoryRepository = new GenericRepository<MemberCategory>(DbContext);
                }
                return memberCategoryRepository;
            }
        }
        public GenericRepository<Member> MemberRepository
        {
            get
            {
                if (this.memberRepository == null)
                {
                    this.memberRepository = new GenericRepository<Member>(DbContext);
                }
                return memberRepository;
            }
        }
        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (this.customerRepository == null)
                {
                    this.customerRepository = new GenericRepository<Customer>(DbContext);
                }
                return customerRepository;
            }
        }
        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new GenericRepository<Order>(DbContext);
                }
                return orderRepository;
            }
        }
        public GenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {
                if (this.orderDetailRepository == null)
                {
                    this.orderDetailRepository = new GenericRepository<OrderDetail>(DbContext);
                }
                return orderDetailRepository;
            }
        }
        public GenericRepository<Comment> CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new GenericRepository<Comment>(DbContext);
                }
                return commentRepository;
            }
        }
        public GenericRepository<QA> QARepository
        {
            get
            {
                if (this.qARepository == null)
                {
                    this.qARepository = new GenericRepository<QA>(DbContext);
                }
                return qARepository;
            }
        }
        public GenericRepository<Emloyee> EmloyeeRepository
        {
            get
            {
                if (this.emloyeeRepository == null)
                {
                    this.emloyeeRepository = new GenericRepository<Emloyee>(DbContext);
                }
                return emloyeeRepository;
            }
        }
        public GenericRepository<EmloyeeType> EmloyeeTypeRepository
        {
            get
            {
                if (this.emloyeeTypeRepository == null)
                {
                    this.emloyeeTypeRepository = new GenericRepository<EmloyeeType>(DbContext);
                }
                return emloyeeTypeRepository;
            }
        }
        public GenericRepository<AccessTimesCount> AccessTimesCountRepository
        {
            get
            {
                if (this.accessTimesCountRepository == null)
                {
                    this.accessTimesCountRepository = new GenericRepository<AccessTimesCount>(DbContext);
                }
                return accessTimesCountRepository;
            }
        }
        public GenericRepository<Cart> CartRepository
        {
            get
            {
                if (this.cartRepository == null)
                {
                    this.cartRepository = new GenericRepository<Cart>(DbContext);
                }
                return cartRepository;
            }
        }
        public GenericRepository<ImportCoupon> ImportCouponRepository
        {
            get
            {
                if (this.importCouponRepository == null)
                {
                    this.importCouponRepository = new GenericRepository<ImportCoupon>(DbContext);
                }
                return importCouponRepository;
            }
        }
        public GenericRepository<ImportCouponDetail> ImportCouponDetailRepository
        {
            get
            {
                if (this.importCouponDetailRepository == null)
                {
                    this.importCouponDetailRepository = new GenericRepository<ImportCouponDetail>(DbContext);
                }
                return importCouponDetailRepository;
            }
        }
        public GenericRepository<ProductViewed> ProductViewedRepository
        {
            get
            {
                if (this.productViewedRepository == null)
                {
                    this.productViewedRepository = new GenericRepository<ProductViewed>(DbContext);
                }
                return productViewedRepository;
            }
        }
        public GenericRepository<Rating> RatingRepository
        {
            get
            {
                if (this.ratingRepository == null)
                {
                    this.ratingRepository = new GenericRepository<Rating>(DbContext);
                }
                return ratingRepository;
            }
        }
        public void Save()
        {
            DbContext.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}