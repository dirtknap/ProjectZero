using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectZero.Startup))]
namespace ProjectZero
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
