using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DwAuthDemo.Startup))]
namespace DwAuthDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
