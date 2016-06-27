using System.Reflection;
using System.Web.Mvc;

namespace EasyIoC.Mvc
{
    /// <summary>
    /// User this initialiser for the default setup, it will register all controllers from the calling assembly.
    /// When creating controllers it will inject parameters registed by any EasyServiceRegistrars in the calling assembly.
    /// </summary>
    public class EasyMvcIoCInitialiser
    {
        /// <summary>
        /// Initialise against the called assembly.
        /// </summary>
        /// <param name="controllerBuilder"></param>
        public void Initialise(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.SetControllerFactory(new EasyMvcControllerFactory(Assembly.GetCallingAssembly()));
        }
    }
}
