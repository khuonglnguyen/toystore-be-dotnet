using System.Web.Mvc;
using ToyStore.Service;
using Unity;
using Unity.Mvc5;

namespace ToyStore
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IProductService,ProductService>();
            container.RegisterType<IProductCategoryService, ProductCategoryService>();
            container.RegisterType<IProducerService, ProducerService>();
            container.RegisterType<ISupplierService, SupplierService>();
            container.RegisterType<IAgeService, AgeService>();
            container.RegisterType<IProductCategoryParentService, ProductCategoryParentService>();
            container.RegisterType<IGenderService, GenderService>();
            container.RegisterType<IMemberService, MemberService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}