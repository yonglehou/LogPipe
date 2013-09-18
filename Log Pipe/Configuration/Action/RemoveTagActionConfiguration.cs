using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Action {
    public class RemoveTagActionConfiguration : IActionConfiguration {
        public RemoveTagActionConfiguration(XElement element) {
            Name = (string) element.Attribute("Name");
        }

        public string Name { get; private set; }
    }
}