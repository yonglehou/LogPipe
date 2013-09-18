using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Patterns {
    public class PatternsConfiguration {
        private readonly Dictionary<string, PatternConfiguration> patterns = new Dictionary<string, PatternConfiguration>();

        public string this[string key] {
            get { return patterns[key].Value; }
        }

        public PatternsConfiguration(XElement element) {
            patterns = element.Elements("Pattern")
                .Select(e => new PatternConfiguration(e))
                .ToDictionary(e => e.Name, e => e);
        }
    }
}