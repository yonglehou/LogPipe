using System.Xml.Linq;
using Common.Logging;

namespace Consortio.Services.LogPipe.Configuration.Output {
    public class ElasticSearchOutputConfiguration : IOutputConfiguration{
        public string Type { get; private set; }

        public string Host { get; private set; }
        public int Port { get; private set; }

        public DefaultDictionary<string, string> TTL { get; private set; }

        public int ConnectionLimit { get; private set; }
        public string IndexNameFormat { get; private set; }

        public ElasticSearchOutputConfiguration(XElement config) {
            Type = (string)config.Attribute("Type");

            Host = config.Element("Host").Value;
            if(string.IsNullOrWhiteSpace(Host))
                throw new ConfigurationException("Invalid 'Host' on " + config);

            int port;
            if(!int.TryParse((string) config.Element("Port") ?? "9200", out port))
                throw new ConfigurationException("Invalid 'Port' for " + config);

            IndexNameFormat = (string) config.Element("IndexNameFormat") ?? @"\l\o\g\p\i\p\e\-yyyyMM";

            Port = port;

            var connectionLimit = config.Element("ConnectionLimit");
            ConnectionLimit = connectionLimit != null ? int.Parse(connectionLimit.Value) : 5;

            InitializeTTL(config);
        }

        private void InitializeTTL(XElement config) {
            TTL = new DefaultDictionary<string, string>();
            var defaultDefined = false;
            foreach (var ttlConfig in config.Elements("TTL")) {
                var typeAttribute = ttlConfig.Attribute("Type");
                if (typeAttribute == null) {
                    if (defaultDefined)
                        throw new ConfigurationException("Default TTL already defined on output: " + config);

                    defaultDefined = true;
                    TTL.DefaultValue = ttlConfig.Value;
                } else {
                    TTL[typeAttribute.Value] = ttlConfig.Value;
                }
            }
        }
    }
}