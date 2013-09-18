using System.Collections.Generic;
using Consortio.Services.LogPipe.Action;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration.Action;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Filter;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe {
    public interface ILogPipeFactory {
        IEnumerable<IFilter> CreateFilters(string type);
        IEnumerable<IPipeline> CreateProcessors();
        IEnumerable<IAction> CreateActions(IEnumerable<IActionConfiguration> configurations);
        IConditionsMatcher CreateConditionMatcher(ConditionsConfiguration conditions);
    }
}