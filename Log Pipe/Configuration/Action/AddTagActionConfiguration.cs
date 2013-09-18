using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Action {
    public class AddTagActionConfiguration : IActionConfiguration {
        public AddTagActionConfiguration(XElement element) {
            Name = (string) element.Attribute("Name");
        }

        public string Name { get; private set; }
    }
}