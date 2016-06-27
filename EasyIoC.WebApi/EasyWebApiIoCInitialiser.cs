using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace EasyIoC.WebApi
{
    /// <summary>
    /// User this initialiser for the default setup, it will register all api controllers from the calling assembly.
    /// When creating controllers it will inject parameters registed by any EasyServiceRegistrars in the calling assembly.
    /// </summary>
    public class EasyWebApiIoCInitialiser
    {
        /// <summary>
        /// Initialise against the called assembly.
        /// </summary>
        /// <param name="configuration"></param>
        public void Initialise(HttpConfiguration configuration)
        {
            configuration.Services.Replace(typeof(IHttpControllerActivator), new EasyWebApiHttpControllerActivator(Assembly.GetCallingAssembly()));
        }
    }
}
