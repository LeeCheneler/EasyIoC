using EasyIoC.Core.Exceptions;
using EasyIoC.Core.ServiceContainer.Entries;
using EasyIoC.Core.ServiceRegistrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyIoC.Core.ServiceContainer
{
    /// <summary>
    /// The default easy service container.
    /// </summary>
    public class EasyServiceContainer : IEasyServiceContainer
    {
        /// <summary>
        /// Create an easy service container.
        /// </summary>
        /// <param name="assembly">Implementations of IServiceRegistrar are loaded from this assembly upon construction.</param>
        public EasyServiceContainer(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            RegisterServiceRegistrars(assembly);
        }


        /// <summary>
        /// Register a service consturcted by its parameterless constructor.
        /// </summary>
        /// <typeparam name="TAbstraction"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        public void Register<TAbstraction, TConcrete>()
        {
            Register(typeof(TAbstraction), typeof(TConcrete));
        }


        /// <summary>
        /// Register a service consturcted by its parameterless constructor.
        /// </summary>
        /// <param name="abstraction"></param>
        /// <param name="concrete"></param>
        public void Register(Type abstraction, Type concrete)
        {
            PreregistrationCheck(abstraction, concrete);

            AddEntry(abstraction.GetHashCode(), new TypeEntry(concrete));
        }


        /// <summary>
        /// Register a service as a singleton.
        /// </summary>
        /// <typeparam name="TAbstraction"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        public void RegisterSingleton<TAbstraction, TConcrete>()
        {
            RegisterSingleton(typeof(TAbstraction), typeof(TConcrete));
        }


        /// <summary>
        /// Register a service as a singleton.
        /// </summary>
        /// <param name="abstraction"></param>
        /// <param name="concrete"></param>
        public void RegisterSingleton(Type abstraction, Type concrete)
        {
            PreregistrationCheck(abstraction, concrete);

            AddEntry(abstraction.GetHashCode(), new SingletonEntry(concrete));
        }


        /// <summary>
        /// Register a service created via a provided func.
        /// </summary>
        /// <typeparam name="TAbstraction"></typeparam>
        /// <param name="func"></param>
        public void Register<TAbstraction>(Func<object> func) 
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            AddEntry(typeof(TAbstraction).GetHashCode(), new FuncEntry(func));
        }
        

        /// <summary>
        /// Register a service created via a provided func as a singleton.
        /// </summary>
        /// <typeparam name="TAbstraction"></typeparam>
        /// <param name="func"></param>
        public void RegisterSingleton<TAbstraction>(Func<object> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            AddEntry(typeof(TAbstraction).GetHashCode(), new SingletonFuncEntry(func));
        }


        /// <summary>
        /// Determine if a service is registered.
        /// </summary>
        /// <typeparam name="TAbstraction"></typeparam>
        /// <returns></returns>
        public bool IsRegistered<TAbstraction>()
        {
            return IsRegistered(typeof(TAbstraction));
        }


        /// <summary>
        /// Determine if a service is registered.
        /// </summary>
        /// <param name="abstraction"></param>
        /// <returns></returns>
        public bool IsRegistered(Type abstraction)
        {
            return _serviceMap.ContainsKey(abstraction.GetHashCode());
        }


        /// <summary>
        /// Activate a service.
        /// </summary>
        /// <typeparam name="TAbstraction"></typeparam>
        /// <returns></returns>
        public object Activate<TAbstraction>()
        {
            return Activate(typeof(TAbstraction));
        }


        /// <summary>
        /// Activate a service.
        /// </summary>
        /// <param name="abstraction"></param>
        /// <returns></returns>
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
