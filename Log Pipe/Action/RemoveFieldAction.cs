using System.Collections.Generic;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Action {
    public class RemoveFieldAction : IAction {
        private readonly RemoveFieldActionConfiguration configuration;

        public RemoveFieldAction(RemoveFieldActionConfiguration configuration) {
            this.configuration = configuration;
        }

        public void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups) {
            evnt.Fields.Remove(configuration.Name);
        }
    }
}