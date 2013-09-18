using System.Collections.Generic;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Action {
    public class RemoveTagAction : IAction {
        private readonly RemoveTagActionConfiguration configuration;

        public RemoveTagAction(RemoveTagActionConfiguration configuration) {
            this.configuration = configuration;
        }

        public void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups) {
            evnt.Tags.Remove(configuration.Name);
        }
    }
}