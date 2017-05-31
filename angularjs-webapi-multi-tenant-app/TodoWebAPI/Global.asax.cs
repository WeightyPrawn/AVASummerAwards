using System.Data.Entity;
using System.Web.Http;
using TodoWebAPI.DAL;

namespace TodoWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<TodoListServiceContext>(new TodoListServiceInitializer());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
