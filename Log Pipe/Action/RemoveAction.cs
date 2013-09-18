using System.Collections.Generic;
using System.Text.RegularExpressions;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Action {
    public class RemoveAction : IAction {
        private readonly RemoveActionConfiguration configuration;
        private readonly IMatchValueExtractor matchValueExtractor;
        private readonly Regex find;

        public RemoveAction(RemoveActionConfiguration configuration, IExpressionBuilder expressionBuilder, IMatchValueExtractor matchValueExtractor)
        {
            this.configuration = configuration;
            this.matchValueExtractor = matchValueExtractor;

            find = expressionBuilder.Build(configuration.Expression);
        }

        public void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups) {
            var originalValue = matchValueExtractor.GetValue(evnt, configuration.Match);
            var newValue = find.Replace(originalValue, "");
            matchValueExtractor.SetValue(evnt, configuration.Match, newValue);
        }
    }
}