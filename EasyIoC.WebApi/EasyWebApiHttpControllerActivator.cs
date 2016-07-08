using EasyIoC.Core.ControllerContainer;
using EasyIoC.Core.ServiceContainer;
using System;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace EasyIoC.WebApi
{
    /// <summary>
    /// EasyIoCs replacement WebApi 2 controller activator. 
    /// Using this controller activator allows you to inject services into controller constructors.
    /// </summary>
    public class EasyWebApiHttpControllerActivator : IHttpControllerActivator
    {
        /// <summary>
        /// Construct EasyWebApiHttpControllerActivator.
        /// </summary>
        /// <param name="assembly"></param>
        public EasyWebApiHttpControllerActivator(Assembly assembly)
        {
            _controllerContainer = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));
            _controllerContainer.RegisterControllers(assembly);
        }


        /// <summary>
        /// Create controller.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="controllerDescriptor"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        /// <remarks>throws 404 HttpException if a suitable controller is not found.</remarks>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType != null)
            {
                return _controllerContainer.Activate(controllerType) as IHttpController;
            }

            throw new HttpException(404, "Not Found");
        }


        private readonly EasyControllerContainer<IHttpController> _controllerContainer;
    }
}
