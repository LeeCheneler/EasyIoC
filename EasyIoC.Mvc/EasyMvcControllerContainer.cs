using EasyIoC.Core;
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
