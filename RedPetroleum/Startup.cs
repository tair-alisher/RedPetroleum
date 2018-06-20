using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RedPetroleum.Startup))]
namespace RedPetroleum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
