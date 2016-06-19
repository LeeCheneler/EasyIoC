using System;

namespace EasyIoC.Core
{
    public interface IEasyServiceContainer
    {
        void Register<TAbstraction, TConcrete>();
        void Register(Type abstraction, Type concrete);
        bool IsRegistered<TAbstraction>();
        bool IsRegistered(Type abstraction);
        object Activate<TAbstraction>();
        object Activate(Type abstraction);
    }
}
