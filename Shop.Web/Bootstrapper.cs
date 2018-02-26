using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using LVT.Web.Core.Fakes;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Web.Core.Authentication;
using Shop.Web.Core;
using Shop.Web.Core.Caching;

namespace Shop.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }    
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerHttpRequest();          
          
            builder.RegisterType<DefaultFormsAuthentication>().As<IFormsAuthentication>().InstancePerHttpRequest();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().InstancePerHttpRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();                  
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));            
        }        
    }
}
