using System.Collections.Generic;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Action {
    public class AddFieldAction : IAction {
        private readonly AddFieldActionConfiguration configuration;

        public AddFieldAction(AddFieldActionConfiguration configuration) {
            this.configuration = configuration;
        }

        public void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups) {
            evnt.Fields[configuration.Name] = configuration.Value;
        }
    }
}