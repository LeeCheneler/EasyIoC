using System;
using System.Linq;
using System.Web.Http.Controllers;

namespace EasyIoC.WebApi
{
    public static class TypeExtensions
    {
        public static bool IsIHttpController(this Type type)
        {
            return type.GetInterfaces().Any(t => t == CONTROLLER_TYPE);
        }

        private static readonly Type CONTROLLER_TYPE = typeof(IHttpController);
    }
}
