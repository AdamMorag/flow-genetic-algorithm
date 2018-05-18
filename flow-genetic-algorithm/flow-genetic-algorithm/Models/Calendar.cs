﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace flow_genetic_algorithm.Models
{    
    public class Calendar
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
    }
}