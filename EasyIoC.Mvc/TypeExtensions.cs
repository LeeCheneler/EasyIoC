using System;
using System.Linq;
using System.Web.Mvc;

namespace EasyIoC.Core
{
    public static class TypeExtensions
    {
        public static bool IsIController(this Type type)
        {
            return type.GetInterfaces().Any(t => t == CONTROLLER_TYPE);
        }

        private static readonly Type CONTROLLER_TYPE = typeof(IController); 
    }
}
