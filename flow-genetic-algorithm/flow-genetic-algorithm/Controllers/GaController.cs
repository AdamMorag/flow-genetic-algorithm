using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace flow_genetic_algorithm.Controllers
{
    public class GaController : ApiController
    {
        [Route("RunGA")]
        [HttpPost]
        public string PostRunGA()
        {
            var gaf = new GafExample();
            return gaf.Test().ToString();
        }
    }
}
