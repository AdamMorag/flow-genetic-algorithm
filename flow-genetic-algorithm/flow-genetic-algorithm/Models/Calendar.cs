using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace flow_genetic_algorithm.Models
{    
    public class Calendar : ICloneable
    {

        [JsonProperty]
        public string _id { get; set; }

        [JsonProperty]
        public string uid { get; set; }

        [JsonProperty]
        public List<Event> events { get; set; }

        public Calendar()
        {
            this.events = new List<Event>();
        }

        public object Clone()
        {
            return new Calendar()
            {
                _id = this._id,
                uid = this.uid,
                events = this.events.Select(e => new Event() {
                    eventId = e.eventId,
                    title = e.title,
                    startDate = e.startDate,
                    endDate = e.endDate
                }).ToList()
            };
        }
    }
}
