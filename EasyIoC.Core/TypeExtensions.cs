using EasyIoC.Core.ServiceRegistrar;
using System;
using System.Linq;

namespace EasyIoC.Core
{
    internal static class TypeExtensions
    {
        internal static bool IsIEasyServiceRegistrar(this Type type)
        {
            return type.GetInterfaces().Contains(IEASYSERVICEREGISTRAR_TYPE);
        }


        private static readonly Type IEASYSERVICEREGISTRAR_TYPE = typeof(IEasyServiceRegistrar);
    }
}
