using EasyIoC.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyIoC.Core
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


        public void Register(Type controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            if (!controller.IsSubclassOf(_baseControllerType)
               && !controller.GetInterfaces().Contains(_baseControllerType))
            {
                throw new TypeMismatchException(controller, _baseControllerType);
            }

            _controllerMap.Add(controller, GetObjectActivator(controller.GetConstructors()[0]));
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
            if (!_controllerMap.ContainsKey(controller))
            {
                throw new NotRegisteredException(controller);
            }

            return _controllerMap[controller].Invoke((object[])controller.GetConstructors()[0].GetParameters().Select(p =>
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


        protected ControllerActivator GetObjectActivator(ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;
                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda = Expression.Lambda(typeof(ControllerActivator), newExp, param);

            //compile it
            return (ControllerActivator)lambda.Compile();
        }


        protected readonly IEasyServiceContainer _serviceContainer;
        protected readonly Dictionary<Type, ControllerActivator> _controllerMap = new Dictionary<Type, ControllerActivator>();
        protected delegate object ControllerActivator(params object[] args);
        private readonly Type _baseControllerType = typeof(TBaseController);
    }
}
