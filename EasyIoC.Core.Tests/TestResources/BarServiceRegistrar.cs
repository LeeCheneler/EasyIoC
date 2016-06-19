using EasyIoC.Core;

namespace EasyIoC.Tests.TestResources
{
    public class BarServiceRegistrar : IEasyServiceRegistrar
    {
        public void RegisterServices(IEasyServiceContainer container)
        {
            container.Register<IBar, Bar>();
        }
    }
}
