using System.Web.Http;

namespace EasyIoC.WebApi.Tests.TestResources
{
    public class FooBarController : ApiController
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
