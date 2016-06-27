using EasyIoC.Core.Exceptions;
using EasyIoC.Core.ServiceContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyIoC.Core.ControllerContainer
{
    /// <summary>
    /// Abstract container to quickly implement controller containers.
    /// </summary>
    /// <typeparam name="TBaseController"></typeparam>
    public abstract class EasyControllerContainer<TBaseController>
    {
        /// <summary>
        /// Construct with a service container to feed parameters into controllers constructor.
        /// </summary>
        /// <param name="serviceContainer"></param>
        public EasyControllerContainer(IEasyServiceContainer serviceContainer)
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException(nameof(serviceContainer));
            }
            _serviceContainer = serviceContainer;
        }
        

        /// <summary>
        /// Register a controller.
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        public void Register<TController>()
        {
            Register(typeof(TController));
        }


        /// <summary>
        /// Register a controller.
        /// </summary>
        /// <param name="controllerType"></param>
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

        
        /// <summary>
        /// Determine if a controller is registered.
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <returns></returns>
        public bool IsRegistered<TController>()
        {
            return IsRegistered(typeof(TController));
        }


        /// <summary>
        /// Determine if a controller is registered.
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public bool IsRegistered(Type controllerType)
        {
            return _controllerMap.ContainsKey(controllerType.GetHashCode());
        }


        /// <summary>
        /// Activate a controller.
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <returns></returns>
        public object Activate<TController>()
        {
            return Activate(typeof(TController));
        }


        /// <summary>
        ///  Activate a controller.
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public object Activate(Type controllerType)
        {
            if (controllerType == null)
            {
                throw new ArgumentNullException(nameof(controllerType));
            }

            var ctors = controllerType.GetConstructors();
            if (ctors.Length > 1)
            {
                throw new MultipleConstructorException(controllerType);
            }

            int key = controllerType.GetHashCode();
            if (!_controllerMap.ContainsKey(key))
            {
                throw new NotRegisteredException(controllerType);
            }

            return _controllerMap[key].Invoke(ctors[0].GetParameters().Select(p =>
            {
                return _serviceContainer.Activate(p.ParameterType);
            }).ToArray());
        }


        /// <summary>
        /// Register controllers in an assembly.
        /// </summary>
        /// <param name="assembly"></param>
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
