using Microsoft.Owin;
using Owin;
using SiSystems.ClientApp.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace SiSystems.ClientApp.Web
{

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
