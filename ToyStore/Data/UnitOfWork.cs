﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data.Repository;
using ToyStore.Models;

namespace ToyStore.Data
{
    public class UnitOfWork : IDisposable
    {
        private ToyStoreDbContext DbContext = new ToyStoreDbContext();
        private GenericRepository<Product> productRepository;
        private GenericRepository<ProductCategory> productCategoryRepository;
        private GenericRepository<Supplier> supplierRepository;
        private GenericRepository<Producer> producerRepository;
        private GenericRepository<Ages> ageRepository;
        private GenericRepository<ProductCategoryParent> productCategoryParentRepository;
        private GenericRepository<Gender> genderParentRepository;
        private GenericRepository<MemberCategory> memberCategoryRepository;
        private GenericRepository<Member> memberRepository;
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
        public GenericRepository<Ages> AgeRepository
        {
            get
            {
                if (this.ageRepository == null)
                {
                    this.ageRepository = new GenericRepository<Ages>(DbContext);
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