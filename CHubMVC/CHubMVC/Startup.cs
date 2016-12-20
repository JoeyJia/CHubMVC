using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CHubMVC.Startup))]
namespace CHubMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
