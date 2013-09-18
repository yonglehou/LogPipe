using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Output {
    public class ConsoleOutputConfiguration : IOutputConfiguration {
        public string Type { get; private set; }

        public ConsoleOutputConfiguration(XElement config) {
            Type = (string)config.Attribute("Type");
        }
    }
}