using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EP.IdentityIsolation.API.SignalrHelper
{
    public class SimpleInjectorHubActivator : IHubActivator
    {
        private readonly Container _container;

        public SimpleInjectorHubActivator(
            Container container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            if (HttpContext.Current == null)
                _container.BeginLifetimeScope();
            return _container.GetInstance(descriptor.HubType) as IHub;
            
            //return (Hub)_container.GetInstance(descriptor.HubType);
        }
    }
}
