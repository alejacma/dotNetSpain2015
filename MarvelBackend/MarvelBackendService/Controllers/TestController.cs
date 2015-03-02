using Microsoft.WindowsAzure.Mobile.Service;
using System.Web.Http;

namespace MarvelBackendService.Controllers
{
    public class TestController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/Test
        public string Get()
        {
            return "Hello";
        }

    }
}
