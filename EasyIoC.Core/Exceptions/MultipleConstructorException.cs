using System;

namespace EasyIoC.Core.Exceptions
{
    public class MultipleConstructorException : Exception
    {
        public MultipleConstructorException(Type type)
            : base($"Can only operate on type with a single constructor. '{type.FullName}' has more than one constructor.")
        {
        }
    }
}
