using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace flow_genetic_algorithm.Controllers
{
    public class ExampleController : ApiController
    {
        [Route("Example")]
        public string GetExample()
        {
            return "Example";
        }
    }
}
