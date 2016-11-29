using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewEarthDirectory.Startup))]
namespace NewEarthDirectory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
