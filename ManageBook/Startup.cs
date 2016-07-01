using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManageBook.Startup))]
namespace ManageBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
