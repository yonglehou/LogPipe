using System.Collections.Generic;
using System.Xml.Linq;
using Common.Logging;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    internal class MultiLineFilterConfiguration : IFilterConfiguration {
        public IEnumerable<IActionConfiguration> Actions { get; private set; }
        public ConditionsConfiguration Conditions { get; private set; }

        public string Type { get { return Conditions.Type; } }

        public MultiLineFilterConfiguration(XElement config, ConfigurationFactory configurationFactory) {
            Conditions = configurationFactory.CreateConditionsConfiguration(config);
            Actions = configurationFactory.CreateActionsConfiguration(config);

            if (string.IsNullOrWhiteSpace(Conditions.Expression))
                throw new ConfigurationException("Invalid 'Expression' in " + config);
        }
    }
}