using System;
using System.Collections.Generic;

namespace Consortio.Services.LogPipe {
    public interface IEvent {
        DateTime Timestamp { get; set; }
        string Type { get; }
        Dictionary<string, string> Fields { get; }
        HashSet<string> Tags { get; }
        string Source { get; set; }
        string Message { get; set; }
        string Sequence { get; }

        string ToString();
    }
}