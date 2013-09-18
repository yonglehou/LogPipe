using System.Collections.Generic;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Action {
    public class ExtractFieldsAction : IAction {
        public ExtractFieldsAction(ExtractFieldsActionConfiguration configuration) {
        }

        public void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups) {
            foreach (var extractedGroup in extractedGroups) {
                evnt.Fields[extractedGroup.Key] = extractedGroup.Value;
            }
        }
    }
}