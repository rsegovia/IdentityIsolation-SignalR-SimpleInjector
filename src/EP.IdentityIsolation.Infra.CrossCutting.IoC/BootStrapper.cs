using EP.IdentityIsolation.Domain.Interface.Repository;
using EP.IdentityIsolation.Infra.CrossCutting.Identity.Configuration;
using EP.IdentityIsolation.Infra.CrossCutting.Identity.Context;
using EP.IdentityIsolation.Infra.CrossCutting.Identity.Model;
using EP.IdentityIsolation.Infra.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SimpleInjector;

namespace EP.IdentityIsolation.Infra.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container, Lifestyle lifestyle)
        {
            container.Register<ApplicationDbContext>(lifestyle);
            container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()), lifestyle);
            container.Register<IRoleStore<IdentityRole, string>>(() => new RoleStore<IdentityRole>(), lifestyle);
            container.Register<ApplicationRoleManager>(lifestyle);
            container.Register<ApplicationUserManager>(lifestyle);
            container.Register<ApplicationSignInManager>(lifestyle);
            container.Register<ISecureDataFormat<AuthenticationTicket>>(() => new FakeTicket(), lifestyle);
            
            container.Register<IUsuarioRepository, UsuarioRepository>(lifestyle);
        } 
    }
}