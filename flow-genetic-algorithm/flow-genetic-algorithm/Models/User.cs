using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using flow_genetic_algorithm.Models;

namespace flow_genetic_algorithm
{    
    public class User
    {
        [JsonProperty]
        public string uid { get; set; }

        [JsonProperty]
        public string name { get; set; }

        [JsonProperty]
        public string image { get; set; }

        [JsonProperty]
        public string color { get; set; }


        public Dictionary<string, Event> taskBestTiming { get; set; }

        public User()
        {
            this.taskBestTiming = new Dictionary<string, Event>();
        }

        public override int GetHashCode()
        {
            return this.uid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
