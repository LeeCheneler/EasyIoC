using EasyIoC.Core.ControllerContainer;
using EasyIoC.Core.ServiceContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EasyIoC.Mvc
{
    /// <summary>
    /// EasyIoCs replacement Mvc 5 controller factory. 
    /// Using this controller factory allows you to inject services into controller constructors.
    /// </summary>
    public class EasyMvcControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Construct EasyMvcControllerFactory.
        /// </summary>
        /// <param name="assembly"></param>
        public EasyMvcControllerFactory(Assembly assembly)
        {
            _controllerContainer = new EasyMvcControllerContainer(new EasyServiceContainer(assembly));
            _controllerContainer.RegisterControllers(assembly);
            var controllerType = typeof(IController);
            foreach (var type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i == controllerType)))
            {
                _controllerTypes.Add(type.FullName.ToLower(), type);
            }
        }


        /// <summary>
        /// Create a controller.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        /// <remarks>throws 404 HttpException if a suitable controller is not found.</remarks>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            string ns = (requestContext.RouteData.DataTokens["Namespaces"] as IEnumerable<string>)?.First().ToLower();
            ns = ns.TrimEnd('*');// areas default namespace ends in a wildcard so need to strip it.
            controllerName = (controllerName + "Controller").ToLower();

            foreach (var pair in _controllerTypes)
            {
                if (pair.Key == ns + "." + controllerName ||
                    (pair.Key.StartsWith(ns) && pair.Key.EndsWith(controllerName)))
                {
                    return _controllerContainer.Activate(pair.Value) as Controller;
                }
            }

            throw new HttpException(404, "Not Found");
        }


        /// <summary>
        /// If the controller is disposable then dispose it.
        /// </summary>
        /// <param name="controller"></param>
        public override void ReleaseController(IController controller)
        {
            (controller as IDisposable)?.Dispose();
        }


        private readonly EasyControllerContainer<IController> _controllerContainer;
        private readonly Dictionary<string, Type> _controllerTypes = new Dictionary<string, Type>();
    }
}
