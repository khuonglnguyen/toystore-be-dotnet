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
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IOrderDetailService, OrderDetailService>();
            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<IQAService, QAService>();
            container.RegisterType<IEmloyeeTypeService, EmloyeeTypeService>();
            container.RegisterType<IEmloyeeService, EmloyeeService>();
            container.RegisterType<IAccessTimesCountService, AccessTimesCountService>();
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<IImportCouponService, ImportCouponService>();
            container.RegisterType<IImportCouponDetailService, ImportCouponDetailService>();
            container.RegisterType<IProductViewedService, ProductViewedService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}