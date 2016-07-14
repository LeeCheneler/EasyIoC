using System.Reflection;
using System.Web.Mvc;

namespace EasyIoC.Mvc
{
    /// <summary>
    /// User this initialiser for the default setup, it will register all controllers from the site assembly passed in.
    /// When creating controllers it will inject parameters registed by any EasyServiceRegistrars in the site assembly.
    /// </summary>
    public class EasyMvcIoCInitialiser
    {
        /// <summary>
        /// Initialise against the site's assembly.
        /// </summary>
        /// <param name="controllerBuilder"></param>
        /// <param name="siteAssembly">Used to source controllers.</param>
        public void Initialise(ControllerBuilder controllerBuilder, Assembly siteAssembly)
        {
            controllerBuilder.SetControllerFactory(new EasyMvcControllerFactory(siteAssembly));
        }
    }
}
