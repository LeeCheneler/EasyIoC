using System.Web.Mvc;

namespace EasyIoC.Mvc.Tests.TestResources
{
    public class BarController : Controller
    {
        public IBar Bar { get; set; }


        public BarController(IBar bar)
        {
            Bar = bar;
        }
    }
}
