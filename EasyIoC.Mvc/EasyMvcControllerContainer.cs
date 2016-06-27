using EasyIoC.Core.ControllerContainer;
using EasyIoC.Core.ServiceContainer;
using System.Web.Mvc;

namespace EasyIoC.Mvc
{
    /// <summary>
    /// Mvc Controller container implementation.
    /// </summary>
    public class EasyMvcControllerContainer : EasyControllerContainer<IController>
    {
        /// <summary>
        /// Construct EasyMvcControllerContainer.
        /// </summary>
        /// <param name="serviceContainer"></param>
        public EasyMvcControllerContainer(IEasyServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }
    }
}
