using EasyIoC.Core;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace EasyIoC.Mvc
{
    public class EasyMvcControllerContainer : BaseEasyControllerContainer
    {
        public EasyMvcControllerContainer(Assembly assembly, IEasyContainer serviceContainer)
            : base(assembly, serviceContainer)
        {
        }


        protected override void RegisterControllers(Assembly assembly)
        {
            var iControllerType = typeof(Controller);
            foreach (var controllerType in assembly.GetTypes().Where(t => t.IsIController()))
            {
                Register(controllerType, controllerType
                    ?.GetConstructors()
                    ?.FirstOrDefault()
                    ?.GetParameters()
                    ?.FirstOrDefault()
                    ?.ParameterType);
            }
        }
    }
}
