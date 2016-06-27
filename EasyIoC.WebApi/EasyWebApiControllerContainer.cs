using EasyIoC.Core.ControllerContainer;
using EasyIoC.Core.ServiceContainer;
using System.Web.Http.Controllers;

namespace EasyIoC.WebApi
{
    /// <summary>
    /// WebApi Controller container implementation.
    /// </summary>
    public class EasyWebApiControllerContainer : EasyControllerContainer<IHttpController>
    {
        /// <summary>
        /// Construct EasyWebApiControllerContainer.
        /// </summary>
        /// <param name="serviceContainer"></param>
        public EasyWebApiControllerContainer(IEasyServiceContainer serviceContainer) 
            : base(serviceContainer)
        {
        }
    }
}
