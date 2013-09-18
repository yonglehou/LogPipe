using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Action {
    public class AddFieldActionConfiguration : IActionConfiguration {
        public AddFieldActionConfiguration(XElement element) {
            Name = (string) element.Attribute("Name");
            Value = (string) element.Attribute("Value");
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}