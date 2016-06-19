using System.Web.Mvc;

namespace EasyIoC.Mvc.Tests.TestResources
{
    public class FooBarController : Controller
    {
        public IFoo Foo { get;set; }
        public IBar Bar { get; set; }


        public FooBarController(IFoo foo, IBar bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}
