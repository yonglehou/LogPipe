using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logging;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    internal class DropFilterConfiguration : IFilterConfiguration {
        public ConditionsConfiguration Conditions { get; private set; }
        public IEnumerable<IActionConfiguration> Actions { get; private set; }

        public string Type { get { return Conditions.Type; } }

        public DropFilterConfiguration(XElement config, ConfigurationFactory configurationFactory) {
            Conditions = configurationFactory.CreateConditionsConfiguration(config);
            Actions = configurationFactory.CreateActionsConfiguration(config);

            if(string.IsNullOrWhiteSpace(Conditions.Expression))
                throw new ConfigurationException("Invalid 'Expression' in " + config);
        }
    }
}