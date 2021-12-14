using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using TableDependency.SqlClient;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        UnitOfWork context = new UnitOfWork();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["SumAccessTimes"] = context.AccessTimesCountRepository.GetAllData().Sum(x => x.AccessTimes);
            Application["RealAccessTimes"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["SumAccessTimes"] = (int)Application["SumAccessTimes"] + 1;
            var accessTimesCount = context.AccessTimesCountRepository.GetAllData().Where(x => x.Date.Date == DateTime.Now.Date);
            if (accessTimesCount.Count() != 0)
            {
                List<AccessTimesCount> list = accessTimesCount.ToList();
                list[0].AccessTimes += 1;
                context.AccessTimesCountRepository.Update(list[0]);
            }
            else
            {
                AccessTimesCount accessTimesCountNew = new AccessTimesCount();
                accessTimesCountNew.Date = DateTime.Now;
                accessTimesCountNew.AccessTimes = 1;
                context.AccessTimesCountRepository.Insert(accessTimesCountNew);
            }
            Application["RealAccessTimes"] = (int)Application["RealAccessTimes"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock();
            Application["RealAccessTimes"] = (int)Application["RealAccessTimes"] - 1;
            Application.UnLock();
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var AccountCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (AccountCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(AccountCookie.Value);
                var role = authTicket.UserData.Split(new Char[] { ',' });
                var userPrincial = new GenericPrincipal(new GenericIdentity(authTicket.Name), role);
                Context.User = userPrincial;
            }
        }
    }
}
