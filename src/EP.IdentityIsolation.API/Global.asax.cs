using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Http;

namespace EP.IdentityIsolation.API
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //GCLatencyMode oldMode = GCSettings.LatencyMode;

            //// Make sure we can always go to the catch block, 
            //// so we can set the latency mode back to `oldMode`
            //RuntimeHelpers.PrepareConstrainedRegions();

            //try
            //{
            //    GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            //    // Generation 2 garbage collection is now
            //    // deferred, except in extremely low-memory situations
            //}
            //finally
            //{
            //    // ALWAYS set the latency mode back
            //    GCSettings.LatencyMode = oldMode;
            //}

            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(DependencyInjector.Register);

            
        }
    }
}