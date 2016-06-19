using System.Web.Http;

namespace EasyIoC.WebApi.Tests.TestResources
{
    public class BarController : ApiController
    {
        public IBar Bar { get; set; }


        public BarController(IBar bar)
        {
            Bar = bar;
        }
    }
}
