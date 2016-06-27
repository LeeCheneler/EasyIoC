using EasyIoC.Core.ControllerContainer;
using EasyIoC.Core.ServiceContainer;
using System.Web.Http.Controllers;

namespace EasyIoC.WebApi
{
    public class EasyWebApiControllerContainer : EasyControllerContainer<IHttpController>
    {
        public EasyWebApiControllerContainer(IEasyServiceContainer serviceContainer) 
            : base(serviceContainer)
        {
        }
    }
}
