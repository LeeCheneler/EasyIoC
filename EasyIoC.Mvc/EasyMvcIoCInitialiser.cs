using System.Reflection;
using System.Web.Mvc;

namespace EasyIoC.Mvc
{
    public class EasyMvcIoCInitialiser
    {
        public void Initialise(ControllerBuilder controllerBuilder)
        {
            controllerBuilder.SetControllerFactory(new EasyMvcControllerFactory(Assembly.GetCallingAssembly()));
        }
    }
}
