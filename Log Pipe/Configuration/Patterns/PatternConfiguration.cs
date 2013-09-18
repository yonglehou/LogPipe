using System.Xml.Linq;
using Common.Logging;

namespace Consortio.Services.LogPipe.Configuration.Patterns {
    public class PatternConfiguration {
        public PatternConfiguration(XElement element) {
            Name = (string) element.Attribute("Name");
            Value = element.Value;

            if (string.IsNullOrWhiteSpace(Name)) {
                throw new ConfigurationException("Missing 'Name' in " + element);
            }

            if (string.IsNullOrWhiteSpace(Value)) {
                throw new ConfigurationException("Missing 'Value' in " + element);
            }
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}