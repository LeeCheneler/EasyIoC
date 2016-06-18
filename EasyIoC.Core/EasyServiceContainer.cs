﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyIoC.Core
{
    public class EasyServiceContainer : IEasyContainer
    {
        public EasyServiceContainer(Assembly assembly)
        {
            RegisterServiceRegistrars(assembly);
        }


        public void Register<TAbstraction, TConcrete>()
        {
            Register(typeof(TAbstraction), typeof(TConcrete));
        }


        public void Register(Type abstraction, Type concrete)
        {
            if (!_serviceMap.ContainsKey(abstraction))
            {
                _serviceMap.Add(abstraction, concrete);
            }
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
            return Activator.CreateInstance(_serviceMap[abstraction]);
        }


        private void RegisterServiceRegistrars(Assembly assembly)
        {
            Type interfaceType = typeof(IEasyServiceRegistrar);
            foreach (Type registrarType in assembly.GetTypes().Where(t => t.IsIEasyServiceRegistrar()))
            {
                (Activator.CreateInstance(registrarType) as IEasyServiceRegistrar)?.RegisterServices(this);
            }
        }


        private readonly Dictionary<Type, Type> _serviceMap = new Dictionary<Type, Type>();
    }
}
