using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flow_genetic_algorithm.Models
{
    public class BoardSuggestionRequest
    {
        [JsonProperty]
        public Board board { get; set; }

        [JsonProperty]
        public IEnumerable<Calendar> usersCalendars { get; set; }
    }
}
