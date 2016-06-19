using System.Web.Mvc;

namespace EasyIoC.Mvc.Tests.TestResources
{
    public class FooController : Controller
    {
        public IFoo Foo { get; set; }


        public FooController(IFoo foo)
        {
            Foo = foo;
        }
    }
}
