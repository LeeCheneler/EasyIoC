using System;

namespace EasyIoC.Core
{
    public interface IEasyContainer
    {
        void Register<TAbstraction, TConcrete>();
        void Register(Type abstraction, Type concrete);
        bool IsRegistered<TAbstraction>();
        bool IsRegistered(Type abstraction);
        object Activate<TAbstraction>();
        object Activate(Type abstraction);
    }
}
