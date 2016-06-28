using System;

namespace EasyIoC.Core.ServiceContainer
{
    public interface IEasyServiceContainer
    {
        void Register<TAbstraction, TConcrete>();
        void Register(Type abstraction, Type concrete);
        void Register<TAbstraction>(Func<object> func);
        void RegisterSingleton<TAbstraction, TConcrete>();
        void RegisterSingleton(Type abstraction, Type concrete);
        void RegisterSingleton<TAbstraction>(Func<object> func);
        bool IsRegistered<TAbstraction>();
        bool IsRegistered(Type abstraction);
        object Activate<TAbstraction>();
        object Activate(Type abstraction);
    }
}
