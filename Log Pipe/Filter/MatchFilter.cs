using System.Collections.Generic;
using System.Linq;
using Consortio.Services.LogPipe.Action;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Filter {
    internal class MatchFilter : IFilter {
        private readonly IEnumerable<IAction> actions;
        private readonly IConditionsMatcher conditionsMatcher;

        public MatchFilter(MatchFilterConfiguration configuration, ILogPipeFactory factory) {
            actions = factory.CreateActions(configuration.Actions).ToList();
            conditionsMatcher = factory.CreateConditionMatcher(configuration.Conditions);
        }

        public FilterFlow Process(ref IEvent evnt) {
            IConditionsMatcherResult result = conditionsMatcher.Process(evnt);
            if (!result.IsMatch)
                return FilterFlow.Continue;

            foreach (IAction action in actions) {
                action.Process(evnt, result.ExtractedGroups);
            }

            return FilterFlow.Continue;
        }
    }
}