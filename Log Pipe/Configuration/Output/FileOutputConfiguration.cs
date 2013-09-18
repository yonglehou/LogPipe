using System.Xml.Linq;
using Common.Logging;

namespace Consortio.Services.LogPipe.Configuration.Output {
    public class FileOutputConfiguration : IOutputConfiguration {
        public FileOutputConfiguration(XElement config) {
            Type = (string) config.Attribute("Type");
            Path = config.Element("Path").Value;

            if (string.IsNullOrWhiteSpace(Path))
                throw new ConfigurationException("Invalid 'Path' in " + config);
        }

        public string Path { get; private set; }
        public string Type { get; private set; }
    }
}