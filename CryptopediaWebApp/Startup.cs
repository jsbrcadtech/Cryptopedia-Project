using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CryptopediaWebApp.Startup))]
namespace CryptopediaWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
