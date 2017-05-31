using System.Configuration;
using System.Threading.Tasks;
using System.Web.Cors;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using TodoWebAPI;

[assembly: OwinStartup(typeof(Startup))]

namespace TodoWebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //additional code to enable CORS
            ConfigureCors(app);

            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Tenant = ConfigurationManager.AppSettings["ida:Tenant"],
                    Audience = ConfigurationManager.AppSettings["ida:Audience"],

                    //additional code for Multi-tenancy
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters() { ValidateIssuer = false }
                });
        }
        private void ConfigureCors(IAppBuilder app)
        {
            var corsPolicy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            var origins = ConfigurationManager.AppSettings["Cors.Origins"];
            if (origins != null)
            {
                foreach (var origin in origins.Split(';'))
                {
                    corsPolicy.Origins.Add(origin);
                }
            }
            else
            {
                corsPolicy.AllowAnyOrigin = true;
            }

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(corsPolicy)
                }
            };

            app.UseCors(corsOptions);
        }
    }
}
