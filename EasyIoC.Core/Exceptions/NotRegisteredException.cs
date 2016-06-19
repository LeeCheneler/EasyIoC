using System;

namespace EasyIoC.Core.Exceptions
{
    public class NotRegisteredException : Exception
    {
        public Type NotRegisteredType { get; private set; }


        public NotRegisteredException(Type type)
            : base($"'{type.FullName}' is not registered.")
        {
            NotRegisteredType = type;
        }
    }
}
