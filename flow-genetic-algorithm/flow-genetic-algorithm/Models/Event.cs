using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace flow_genetic_algorithm.Models
{    
    public class Event
    {
        [JsonProperty]
        public string eventId { get; set; }

        [JsonProperty]
        public DateTime startDate { get; set; }

        [JsonProperty]
        public DateTime endDate { get; set; }

        [JsonProperty]
        public string title { get; set; }

        public bool doesEventsOverlapping(Event otherEvent)
        {
            return otherEvent.startDate.ToLocalTime() < this.endDate && otherEvent.endDate.ToLocalTime() > this.startDate;            
        }
    }
}
