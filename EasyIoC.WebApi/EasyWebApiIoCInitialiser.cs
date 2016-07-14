using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace EasyIoC.WebApi
{
    /// <summary>
    /// User this initialiser for the default setup, it will register all api controllers from the site assembly passed in.
    /// When creating controllers it will inject parameters registed by any EasyServiceRegistrars in the site assembly.
    /// </summary>
    public class EasyWebApiIoCInitialiser
    {
        /// <summary>
        /// Initialise against the site's assembly.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="siteAssembly">Used to source controllers.</param>
        public void Initialise(HttpConfiguration configuration, Assembly siteAssembly)
        {
            configuration.Services.Replace(typeof(IHttpControllerActivator), new EasyWebApiHttpControllerActivator(siteAssembly));
        }
    }
}
