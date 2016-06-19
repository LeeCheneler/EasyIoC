using EasyIoC.Core;

namespace EasyIoC.WebApi.Tests.TestResources
{
    public class BarServiceRegistrar : IEasyServiceRegistrar
    {
        public void RegisterServices(IEasyServiceContainer container)
        {
            container.Register<IBar, Bar>();
            container.Register<IFoo, Foo>();
        }
    }
}
