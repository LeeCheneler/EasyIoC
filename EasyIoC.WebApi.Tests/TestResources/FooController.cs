using System.Web.Http;

namespace EasyIoC.WebApi.Tests.TestResources
{
    public class FooController : ApiController
    {
        public IFoo Foo { get; set; }


        public FooController(IFoo foo)
        {
            Foo = foo;
        }
    }
}
