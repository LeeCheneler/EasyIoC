using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace EasyIoC.WebApi
{
    public class EasyWebApiIoCInitialiser
    {
        public void Initialise(HttpConfiguration configuration)
        {
            configuration.Services.Replace(typeof(IHttpControllerActivator), new EasyWebApiHttpControllerActivator(Assembly.GetCallingAssembly()));
        }
    }
}
