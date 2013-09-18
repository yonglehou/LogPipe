using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Common.Logging;

namespace Consortio.Services.LogPipe.Configuration.Input {
    public class FileInputConfiguration : IInputConfiguration {
        public FileInputConfiguration(XElement config) {
            Type = config.Attribute("Type").Value;
            Tags = new HashSet<string>(config.Element("Tags").Elements("Tag").Select(t => t.Value));
            Path = config.Element("Path").Value;

            if (string.IsNullOrWhiteSpace(Type))
                throw new ConfigurationException("Missing 'Type' in " + config);

            if(string.IsNullOrWhiteSpace("Path"))
                throw new ConfigurationException("Missing 'Path' in " + config);

            var interval = (string)config.Element("Interval") ?? "00:00:05";
            Interval = TimeSpan.Parse(interval);

            Encoding = Encoding.GetEncoding((string)config.Element("Encoding") ?? "UTF-8");
        }

        public string Type { get; private set; }
        public HashSet<string> Tags { get; private set; }
        public string Path { get; private set; }
        public TimeSpan Interval { get; private set; }
        public Encoding Encoding { get; private set; }
    }
}