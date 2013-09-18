using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Action {
    public class RemoveActionConfiguration : IActionConfiguration {
        public RemoveActionConfiguration(XElement element) {
            Expression = (string) element.Element("Expression");
            Match = (string) element.Element("Expression").Attribute("Match");
        }

        public string Match { get; set; }
        public string Expression { get; private set; }
    }
}