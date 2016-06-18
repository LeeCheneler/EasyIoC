using System;
using System.Linq;

namespace EasyIoC.Core
{
    public static class TypeExtensions
    {
        public static bool IsIEasyServiceRegistrar(this Type type)
        {
            return type.GetInterfaces().Contains(IEASYSERVICEREGISTRAR_TYPE);
        }


        private static readonly Type IEASYSERVICEREGISTRAR_TYPE = typeof(IEasyServiceRegistrar);
    }
}
