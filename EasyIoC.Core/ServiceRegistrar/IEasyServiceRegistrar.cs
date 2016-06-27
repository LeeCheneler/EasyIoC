using EasyIoC.Core.ServiceContainer;

namespace EasyIoC.Core.ServiceRegistrar
{
    /// <summary>
    /// Create concrete registrars in your assembly to register services within.
    /// </summary>
    public interface IEasyServiceRegistrar
    {
        /// <summary>
        /// Register services in the given container.
        /// </summary>
        /// <param name="container"></param>
        void RegisterServices(IEasyServiceContainer container);
    }
}
