using System;

namespace EasyIoC.Core.Exceptions
{
    public class TypeMismatchException : Exception
    {
        public TypeMismatchException(Type abstraction, Type concrete)
            : base($"'{concrete.FullName}' does not implement {abstraction.FullName}.")
        {
        }
    }
}
