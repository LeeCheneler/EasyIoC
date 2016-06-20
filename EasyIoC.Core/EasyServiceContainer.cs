using EasyIoC.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyIoC.Core
{
    public class EasyServiceContainer : IEasyServiceContainer
    {
        public EasyServiceContainer(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            RegisterServiceRegistrars(assembly);
        }


        public void Register<TAbstraction, TConcrete>()
        {
            Register(typeof(TAbstraction), typeof(TConcrete));
        }


        public void Register(Type abstraction, Type concrete)
        {
            if (abstraction == null)
            {
                throw new ArgumentNullException(nameof(abstraction));
            }
            if (concrete == null)
            {
                throw new ArgumentNullException(nameof(concrete));
            }

            if (!concrete.IsSubclassOf(abstraction)
                && !concrete.GetInterfaces().Contains(abstraction))
            {
                throw new TypeMismatchException(abstraction, concrete);
            }

            _controllerMap.Add(abstraction, GetServiceActivator(concrete.GetConstructors()[0]));
        }


        public bool IsRegistered<TAbstraction>()
        {
            return IsRegistered(typeof(TAbstraction));
        }


        public bool IsRegistered(Type abstraction)
        {
            return _serviceMap.ContainsKey(abstraction);
        }


        public object Activate<TAbstraction>()
        {
            return Activate(typeof(TAbstraction));
        }


        public object Activate(Type abstraction)
        {
            if (!_serviceMap.ContainsKey(abstraction))
            {
                throw new NotRegisteredException(abstraction);
            }
            return _serviceMap[abstraction].Invoke();
        }


        private void RegisterServiceRegistrars(Assembly assembly)
        {
            Type interfaceType = typeof(IEasyServiceRegistrar);
            foreach (Type registrarType in assembly.GetTypes().Where(t => t.IsIEasyServiceRegistrar()))
            {
                (Activator.CreateInstance(registrarType) as IEasyServiceRegistrar)?.RegisterServices(this);
            }
        }


        private ServiceActivator GetServiceActivator(ConstructorInfo ctor)
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
            LambdaExpression lambda = Expression.Lambda(typeof(ServiceActivator), newExp, param);

            //compile it
            return (ServiceActivator)lambda.Compile();
        }


        private readonly Dictionary<Type, ServiceActivator> _controllerMap = new Dictionary<Type, ServiceActivator>();
        private delegate object ServiceActivator(params object[] args);
        private readonly Dictionary<Type, ServiceActivator> _serviceMap = new Dictionary<Type, ServiceActivator>();
    }
}
