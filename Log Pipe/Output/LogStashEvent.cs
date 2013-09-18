using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Consortio.Services.LogPipe.Output {
    public class LogStashEvent {
        private readonly IEvent evnt;

        public LogStashEvent(IEvent evnt) {
            this.evnt = evnt;
        }

        [JsonProperty(PropertyName = "_ttl")]
        public string TTL { get; set; }

        [JsonProperty(PropertyName = "_id")]
        public string Id { get { return evnt.Sequence; } }

        [JsonProperty(PropertyName = "@timestamp")]
        public DateTime Timestamp {
            get { return evnt.Timestamp; }
        }

        [JsonProperty(PropertyName = "@type")]
        public string Type {
            get { return evnt.Type; }
        }

        [JsonProperty(PropertyName = "@fields")]
        public Dictionary<string, string> Fields {
            get { return evnt.Fields; }
        }

        [JsonProperty(PropertyName = "@tags")]
        public HashSet<string> Tags {
            get { return evnt.Tags; }
        }

        [JsonProperty(PropertyName = "@source")]
        public string Source {
            get { return evnt.Source; }
        }

        [JsonProperty(PropertyName = "@message")]
        public string Message {
            get { return evnt.Message; }
        }
    }
}