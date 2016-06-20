using System;

namespace EasyIoC.Core.Exceptions
{
    public class AlreadyRegisteredException : Exception
    {
        public Type RegisteredType { get; set; }

        
        public AlreadyRegisteredException(Type type)
            : base($"{type.FullName} is already registered.")
        {
            RegisteredType = type;
        }
    }
}
