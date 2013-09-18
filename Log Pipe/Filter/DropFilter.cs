using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Filter {
    internal class DropFilter : IFilter {
        private readonly IConditionsMatcher conditionsMatcher;

        public DropFilter(DropFilterConfiguration configuration, ILogPipeFactory logPipeFactory) {
            conditionsMatcher = logPipeFactory.CreateConditionMatcher(configuration.Conditions);
        }

        public FilterFlow Process(ref IEvent evnt) {
            return conditionsMatcher.Process(evnt).IsMatch ? FilterFlow.Drop : FilterFlow.Continue;
        }
    }
}