using EasyIoC.Core.Exceptions;
using System;
using System.Linq;

namespace EasyIoC.Core.ServiceContainer.Entries
{
    internal class TypeEntry : IEntry
    {
        public TypeEntry(Type type)
        {
            var ctor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);
            if (ctor == null)
            {
                throw new NoParameterlessConstructorException(type);
            }
            _activator = new ServiceActivatorBuilder().Create(ctor);
        }


        public object GetService()
        {
            return _activator.Invoke();
        }

        
        private ServiceActivator _activator;
    }
}
