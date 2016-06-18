using EasyIoC.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyIoC.Mvc
{
    public abstract class BaseEasyControllerContainer : IEasyContainer
    {
        public BaseEasyControllerContainer(Assembly assembly, IEasyContainer serviceContainer)
        {
            RegisterControllers(assembly);
            _serviceContainer = serviceContainer;
        }


        public void Register(Type controller, Type ctorArg)
        {
            _controllerMap.Add(controller, ctorArg);
        }


        public void Register<TController, TCtorArg>()
        {
            Register(typeof(TController), typeof(TCtorArg));
        }


        public bool IsRegistered<TController>()
        {
            return IsRegistered(typeof(TController));
        }


        public bool IsRegistered(Type controller)
        {
            return _controllerMap.ContainsKey(controller);
        }


        public object Activate<TController>()
        {
            return Activate(typeof(TController));
        }


        public object Activate(Type controller)
        {
            if (_controllerMap[controller] != null)
            {
                return Activator.CreateInstance(controller, _serviceContainer.Activate(_controllerMap[controller]));
            }

            return Activator.CreateInstance(controller);
        }


        protected abstract void RegisterControllers(Assembly assembly);


        protected readonly IEasyContainer _serviceContainer;
        protected readonly Dictionary<Type, Type> _controllerMap = new Dictionary<Type, Type>();
    }
}
