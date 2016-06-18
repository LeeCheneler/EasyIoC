using EasyIoC.Core;
using System;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace EasyIoC.WebApi
{
    public class EasyWebApiHttpControllerActivator : IHttpControllerActivator
    {
        public EasyWebApiHttpControllerActivator(Assembly assembly)
        {
            _controllerContainer = new EasyWebApiControllerContainer(assembly, new EasyServiceContainer(assembly));
        }


        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType != null)
            {
                return _controllerContainer.Activate(controllerType) as IHttpController;
            }

            throw new HttpException(404, "Not Found");
        }


        private readonly IEasyContainer _controllerContainer;
    }
}
