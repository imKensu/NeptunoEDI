using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NeptunoMVC01.Startup))]
namespace NeptunoMVC01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
