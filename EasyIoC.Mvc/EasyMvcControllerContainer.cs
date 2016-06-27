using EasyIoC.Core.ControllerContainer;
using EasyIoC.Core.ServiceContainer;
using System.Web.Mvc;

namespace EasyIoC.Mvc
{
    public class EasyMvcControllerContainer : EasyControllerContainer<IController>
    {
        public EasyMvcControllerContainer(IEasyServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }
    }
}
