using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyCalendar.Startup))]
namespace MyCalendar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
