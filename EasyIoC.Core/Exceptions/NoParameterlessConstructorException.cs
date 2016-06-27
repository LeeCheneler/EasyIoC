using System;

namespace EasyIoC.Core.Exceptions
{
    public class NoParameterlessConstructorException : Exception
    {
        public NoParameterlessConstructorException(Type type)
            : base($"No parameterless constructor for '{type.FullName}'. Services registered must have a parameterless constructor unless they're created via a provided Func<T>.")
        {
        }
    }
}
