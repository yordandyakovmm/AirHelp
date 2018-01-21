using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AirHelp.Startup))]
namespace AirHelp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
