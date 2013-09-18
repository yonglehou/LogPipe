using System.Collections.Generic;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    public interface IFilterConfiguration : ITypeConfiguration {
        IEnumerable<IActionConfiguration> Actions { get; }
        ConditionsConfiguration Conditions { get; }
    }
}