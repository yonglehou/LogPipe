using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    public class ExtractTimestampFilterConfiguration : IFilterConfiguration {
        public DateTimeStyles DateTimeStyles { get; private set; }
        
        public IEnumerable<IActionConfiguration> Actions { get; set; }
        public ConditionsConfiguration Conditions { get; set; }

        public string Type { get { return Conditions.Type; } }

        public ExtractTimestampFilterConfiguration(XElement config, ConfigurationFactory configurationFactory) {
            var adjustToLocal = (bool?) config.Attribute("AdjustToLocal") ?? false;
            if(!adjustToLocal)
                DateTimeStyles |= DateTimeStyles.AdjustToUniversal;
            
            var assumeLocal = (bool?) config.Attribute("AssumeLocal") ?? false;
            if (assumeLocal)
                DateTimeStyles |= DateTimeStyles.AssumeLocal;
            else
                DateTimeStyles |= DateTimeStyles.AssumeUniversal;

            Conditions = configurationFactory.CreateConditionsConfiguration(config);
            Actions = configurationFactory.CreateActionsConfiguration(config);
        }
    }
}