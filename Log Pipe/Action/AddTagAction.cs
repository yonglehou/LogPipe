using System.Collections.Generic;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Action {
    internal class AddTagAction : IAction {
        private readonly AddTagActionConfiguration configuration;

        public AddTagAction(AddTagActionConfiguration configuration) {
            this.configuration = configuration;
        }

        public void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups) {
            evnt.Tags.Add(configuration.Name);
        }
    }
}