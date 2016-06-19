using EasyIoC.Core;

namespace EasyIoC.Mvc.Tests.TestResources
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
