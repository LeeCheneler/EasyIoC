using EasyIoC.Core;
using EasyIoC.Mvc;
using System.Linq;
using System.Reflection;

namespace EasyIoC.WebApi
{
    public class EasyWebApiControllerContainer : BaseEasyControllerContainer
    {
        public EasyWebApiControllerContainer(Assembly assembly, IEasyContainer serviceContainer) 
            : base(assembly, serviceContainer)
        {
        }


        protected override void RegisterControllers(Assembly assembly)
        {
            foreach (var controllerType in assembly.GetTypes().Where(t => t.IsIHttpController()))
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
