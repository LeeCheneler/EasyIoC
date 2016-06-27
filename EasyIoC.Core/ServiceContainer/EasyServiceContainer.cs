using EasyIoC.Core.Exceptions;
using EasyIoC.Core.ServiceContainer.Entries;
using EasyIoC.Core.ServiceRegistrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyIoC.Core.ServiceContainer
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
            PreregistrationCheck(abstraction, concrete);

            AddEntry(abstraction.GetHashCode(), new TypeEntry(concrete));
        }


        public void RegisterSingleton<TAbstraction, TConcrete>()
        {
            RegisterSingleton(typeof(TAbstraction), typeof(TConcrete));
        }


        public void RegisterSingleton(Type abstraction, Type concrete)
        {
            PreregistrationCheck(abstraction, concrete);

            AddEntry(abstraction.GetHashCode(), new SingletonEntry(concrete));
        }


        public void Register<TAbstraction>(Func<object> func) 
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            AddEntry(typeof(TAbstraction).GetHashCode(), new FuncEntry(func));
        }


        public bool IsRegistered<TAbstraction>()
        {
            return IsRegistered(typeof(TAbstraction));
        }


        public bool IsRegistered(Type abstraction)
        {
            return _serviceMap.ContainsKey(abstraction.GetHashCode());
        }


        public object Activate<TAbstraction>()
        {
            return Activate(typeof(TAbstraction));
        }


        public object Activate(Type abstraction)
        {
            if (abstraction == null)
            {
                throw new ArgumentNullException(nameof(abstraction));
            }

            int key = abstraction.GetHashCode();
            if (_serviceMap.ContainsKey(key))
            {
                return _serviceMap[key].GetService();
            }

            throw new NotRegisteredException(abstraction);
        }
        

        private void PreregistrationCheck(Type abstraction, Type concrete)
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

            if (IsRegistered(abstraction))
            {
                throw new AlreadyRegisteredException(abstraction);
            }
        }


        private void RegisterServiceRegistrars(Assembly assembly)
        {
            Type interfaceType = typeof(IEasyServiceRegistrar);
            foreach (Type registrarType in assembly.GetTypes().Where(t => t.IsIEasyServiceRegistrar()))
            {
                (Activator.CreateInstance(registrarType) as IEasyServiceRegistrar)?.RegisterServices(this);
            }
        }


        private void AddEntry(int key, IEntry entry)
        {
            _serviceMap.Add(key, entry);
        }


        private readonly Dictionary<int, IEntry> _serviceMap = new Dictionary<int, IEntry>();
    }
}
