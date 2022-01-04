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
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IOrderDetailService, OrderDetailService>();
            container.RegisterType<IQAService, QAService>();
            container.RegisterType<IUserTypeService, UserTypeService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAccessTimesCountService, AccessTimesCountService>();
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<IImportCouponService, ImportCouponService>();
            container.RegisterType<IImportCouponDetailService, ImportCouponDetailService>();
            container.RegisterType<IProductViewedService, ProductViewedService>();
            container.RegisterType<IRatingService, RatingService>();
            container.RegisterType<IDecentralizationService, DecentralizationService>();
            container.RegisterType<IDiscountCodeService, DiscountCodeService>();
            container.RegisterType<IDiscountCodeDetailService, DiscountCodeDetailService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IUserDiscountCodeService, UserDiscountCodeService>();
            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IUsersSpinService, UsersSpinService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}