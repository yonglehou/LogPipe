using System.Collections.Generic;
using System.Xml.Linq;
using Autofac;
using Consortio.Services.LogPipe.Configuration.Action;
using Consortio.Services.LogPipe.Configuration.Filter;

namespace Consortio.Services.LogPipe.Configuration {
    public class ConfigurationFactory {
        private readonly ILifetimeScope lifetimeScope;

        public ConfigurationFactory(ILifetimeScope lifetimeScope) {
            this.lifetimeScope = lifetimeScope;
        }

        public IEnumerable<IActionConfiguration> CreateActionsConfiguration(XElement filterElement) {
            return new ActionsConfiguration(filterElement.Element("Actions"), lifetimeScope);
        }

        public ConditionsConfiguration CreateConditionsConfiguration(XElement filterElement) {
            return new ConditionsConfiguration(filterElement.Element("Conditions"));
        }
    }
}