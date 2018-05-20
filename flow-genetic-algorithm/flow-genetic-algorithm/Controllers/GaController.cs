using flow_genetic_algorithm.Models;
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
        public Dictionary<string, List<Models.Task>> PostRunGA(BoardSuggestionRequest request)
        {            
            var gaf = new FlowGeneticAlgorithm(request.board, request.usersCalendars);
            return gaf.GetBoardSuggestion().tasks.GroupBy(t => t.owner.uid).
                ToDictionary(k => k.Key, v => v.ToList());
        }
    }
}
