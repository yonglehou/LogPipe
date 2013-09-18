using System.Collections.Generic;

namespace Consortio.Services.LogPipe.Action {
    public interface IAction {
        void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups);
    }
}