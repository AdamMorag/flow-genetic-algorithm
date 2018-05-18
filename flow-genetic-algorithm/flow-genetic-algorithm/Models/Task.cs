using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace flow_genetic_algorithm.Models
{    
    public class Task
    {
        [JsonProperty]
        public string boardId { get; set; }

        [JsonProperty]
        public string taskId { get; set; }

        [JsonProperty]
        public string title { get; set; }

        [JsonProperty]
        public string boardName { get; set; }

        [JsonProperty]
        public string status { get; set; }

        [JsonProperty]
        public double overallTime { get; set; }

        [JsonProperty]
        public double remainingTime { get; set; }

        [JsonProperty]
        public User owner { get; set; }        
    }
}
