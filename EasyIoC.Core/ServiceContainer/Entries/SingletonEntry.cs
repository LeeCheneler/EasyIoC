using EasyIoC.Core.Exceptions;
using System;
using System.Linq;

namespace EasyIoC.Core.ServiceContainer.Entries
{
    internal class SingletonEntry : IEntry
    {
        public SingletonEntry(Type type)
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
            if (_instance == null)
            {
                _instance = _activator.Invoke();
            }

            return _instance;
        }


        private ServiceActivator _activator;
        private object _instance = null;
    }
}
