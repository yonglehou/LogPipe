using System.Collections.Generic;
using System.Xml.Linq;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    internal class MatchFilterConfiguration : IFilterConfiguration {
        public IEnumerable<IActionConfiguration> Actions { get; set; }
        public ConditionsConfiguration Conditions { get; set; }

        public string Type { get { return Conditions.Type; } }

        public MatchFilterConfiguration(XElement config, ConfigurationFactory configurationFactory) {
            Conditions = configurationFactory.CreateConditionsConfiguration(config);
            Actions = configurationFactory.CreateActionsConfiguration(config);
        }
    }
}