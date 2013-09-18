using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Action {
    public class ReplaceActionConfiguration : IActionConfiguration {
        public ReplaceActionConfiguration(XElement element) {
            Expression = (string) element.Element("Expression");
            Match = (string) element.Element("Expression").Attribute("Match");
            With = (string) element.Element("With");
        }

        public string Match { get; private set; }
        public string Expression { get; private set; }

        public string With { get; private set; }
    }
}