using EasyIoC.Core.ServiceContainer;

namespace EasyIoC.Core.ServiceRegistrar
{
    public interface IEasyServiceRegistrar
    {
        void RegisterServices(IEasyServiceContainer container);
    }
}
