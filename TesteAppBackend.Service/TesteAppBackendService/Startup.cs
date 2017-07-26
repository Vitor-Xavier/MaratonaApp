using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TesteAppBackendService.Startup))]

namespace TesteAppBackendService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}