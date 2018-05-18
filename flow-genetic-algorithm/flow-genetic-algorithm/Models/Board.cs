using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace flow_genetic_algorithm.Models
{    
    public class Board
    {
        [JsonProperty]
        public string _id { get; set; }

        [JsonProperty]
        public string boardId { get; set; }

        [JsonProperty]
        public string title { get; set; }

        [JsonProperty]
        public DateTime startDate { get; set; }

        [JsonProperty]
        public DateTime endDate { get; set; }

        [JsonProperty]
        public User boardOwner { get; set; }

        [JsonProperty]
        public List<User> boardMembers { get; set; }
        
        [JsonProperty]
        public List<Task> tasks { get; set; }

    }
}
