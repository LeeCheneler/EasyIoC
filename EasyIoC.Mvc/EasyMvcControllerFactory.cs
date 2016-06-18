using EasyIoC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EasyIoC.Mvc
{
    public class EasyMvcControllerFactory : DefaultControllerFactory
    {
        public EasyMvcControllerFactory(Assembly assembly)
        {
            _controllerContainer = new EasyMvcControllerContainer(assembly, new EasyServiceContainer(assembly));
            foreach (var type in assembly.GetTypes().Where(t => t.IsIController()))
            {
                _controllerTypes.Add(type.FullName.ToLower(), type);
            }
        }


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


        public override void ReleaseController(IController controller)
        {
            (controller as IDisposable)?.Dispose();
        }


        private readonly IEasyContainer _controllerContainer;
        private readonly Dictionary<string, Type> _controllerTypes = new Dictionary<string, Type>();
    }
}
