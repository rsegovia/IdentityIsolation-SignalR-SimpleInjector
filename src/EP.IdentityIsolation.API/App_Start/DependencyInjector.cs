using System.Web;
using System.Web.Http;
using EP.IdentityIsolation.API.Hubs;
using EP.IdentityIsolation.API.SignalrHelper;
using EP.IdentityIsolation.Infra.CrossCutting.Identity.Configuration;
using EP.IdentityIsolation.Infra.CrossCutting.IoC;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.WebApi;

namespace EP.IdentityIsolation.API
{
    public static class DependencyInjector
    {
        public static void Register(HttpConfiguration config)
        {
            //var hybrid = Lifestyle.CreateHybrid(
            //    () => Startup.container.GetCurrentExecutionContextScope() != null,
            //    new ExecutionContextScopeLifestyle(),
            //    new SimpleInjector.Integration.Web.WebRequestLifestyle());


            //var hybrid = Lifestyle.CreateHybrid(
            //    () => Startup.container.GetCurrentExecutionContextScope() != null,
            //    new SimpleInjector.Integration.Web.WebRequestLifestyle(),
            //    new ExecutionContextScopeLifestyle());

            //var lifestyle = Lifestyle.CreateHybrid(
            //    lifestyleSelector: () => HttpContext.Current != null,
            //    trueLifestyle: new WebRequestLifestyle(),
            //    falseLifestyle: Lifestyle.Transient);


            var lifestyle = Lifestyle.CreateHybrid(
                () => Startup.container.GetCurrentExecutionContextScope() != null,
                new WebApiRequestLifestyle(), 
                Lifestyle.Transient);


            Startup.container = new Container();
            Startup.container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            //Startup.container.Options.DefaultScopedLifestyle = hybrid;

            // Chamada dos módulos do Simple Injector
            BootStrapper.RegisterServices(Startup.container, lifestyle);
            Startup.container.Register<ChatHub,ChatHub>(lifestyle);

            //Startup.container.RegisterPerWebRequest<ChatHub>();
            // Aqui está o bug.. não consigo registrar isso de forma alguma
            //Startup.container.Register<ChatHub>(hybrid);
            

            // Necessário para registrar o ambiente do Owin que é dependência do Identity
            // Feito fora da camada de IoC para não levar o System.Web para fora
            Startup.container.RegisterPerWebRequest(() =>
            {
                if (HttpContext.Current != null && HttpContext.Current.Items["owin.Environment"] == null && Startup.container.IsVerifying())
                {
                    return new OwinContext().Authentication;
                }
                return HttpContext.Current.GetOwinContext().Authentication;

            });

            // This is an extension method from the integration package.
            Startup.container.RegisterWebApiControllers(config);

            Startup.container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(Startup.container);

            var activator = new SimpleInjectorHubActivator(Startup.container);
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => activator);
            //GlobalHost.DependencyResolver.Register(typeof(ChatHub), () => activator);


        }
    }
}