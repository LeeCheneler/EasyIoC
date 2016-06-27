using EasyIoC.Core.Exceptions;
using EasyIoC.Core.ServiceContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyIoC.Core.ControllerContainer
{
    public class EasyControllerContainer<TBaseController>
    {
        public EasyControllerContainer(IEasyServiceContainer serviceContainer)
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException(nameof(serviceContainer));
            }
            _serviceContainer = serviceContainer;
        }
        

        public void Register<TController>()
        {
            Register(typeof(TController));
        }


        public void Register(Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }
            if (!controllerType.IsSubclassOf(_baseControllerType)
               && !controllerType.GetInterfaces().Contains(_baseControllerType))
            {
                throw new TypeMismatchException(controllerType, _baseControllerType);
            }
            
            _controllerMap.Add(controllerType.GetHashCode(), new ServiceActivatorBuilder().Create(controllerType.GetConstructors()[0]));
        }

        
        public bool IsRegistered<TController>()
        {
            return IsRegistered(typeof(TController));
        }


        public bool IsRegistered(Type controllerType)
        {
            return _controllerMap.ContainsKey(controllerType.GetHashCode());
        }


        public object Activate<TController>()
        {
            return Activate(typeof(TController));
        }


        public object Activate(Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            int key = controllerType.GetHashCode();
            if (!_controllerMap.ContainsKey(key))
            {
                throw new NotRegisteredException(controllerType);
            }

            return _controllerMap[key].Invoke(controllerType.GetConstructors()[0].GetParameters().Select(p =>
            {
                return _serviceContainer.Activate(p.ParameterType);
            }).ToArray());
        }


        public void RegisterControllers(Assembly assembly)
        {
            var baseControllerType = typeof(TBaseController);
            foreach (var controllerType in assembly.GetTypes().Where(type => type.GetInterfaces().Any(t => t == baseControllerType)))
            {
                Register(controllerType);
            }
        }
        

        protected readonly IEasyServiceContainer _serviceContainer;
        private readonly Dictionary<int, ServiceActivator> _controllerMap = new Dictionary<int, ServiceActivator>();
        private readonly Type _baseControllerType = typeof(TBaseController);
    }
}
