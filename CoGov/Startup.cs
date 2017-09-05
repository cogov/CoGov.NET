using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoGov.Startup))]
namespace CoGov
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
