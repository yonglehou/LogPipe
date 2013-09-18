using System;
using System.Collections.Generic;
using System.Linq;

namespace Consortio.Services.LogPipe {
    public class Event : IEvent {
        private Dictionary<string, string> fields;
        private HashSet<string> tags;

        public Event(string message, string inputType, string source, DateTime timestamp, HashSet<string> tags = null) {
            this.tags = tags;
            Message = message;
            Type = inputType;
            Source = source;
            Timestamp = timestamp;
        }

        public string Type { get; private set; }

        public DateTime Timestamp { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Sequence { get; set; }

        public Dictionary<string, string> Fields {
            get { return fields ?? (fields = new Dictionary<string, string>()); }
        }

        public HashSet<string> Tags {
            get { return tags ?? (tags = new HashSet<string>()); }
        }

        public override string ToString() {
            return string.Format("Message:{0}, Source:{1}, Tags:{2}, Type:{3}, Timestamp:{4}, Fields:{5}",
                Message,
                Source,
                string.Join(", ", Tags),
                Type,
                Timestamp,
                string.Join(", ", Fields.Select(f => "{ '" + f.Key + "': '" + f.Value + "' }"))
            );
        }
    }
}