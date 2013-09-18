using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    public class ConditionsConfiguration : ITypeConfiguration {
        public string Expression { get; private set; }
        public bool Negate { get; private set; }
        public string Match { get; private set; }
        public string Type { get; set; }

        public IEnumerable<string> MustHaveAllTags { get; private set; }
        public IEnumerable<string> MustHaveOneTag { get; private set; }

        public ConditionsConfiguration(XElement config) {
            if (config == null) {
                MustHaveAllTags = new string[0];
                MustHaveOneTag = new string[0];
                return;
            }

            Type = config.Element("Type") != null ? config.Element("Type").Value : null;
            Expression = (string)config.Element("Expression");
            Match = Expression != null ? (string)config.Element("Expression").Attribute("Match") : null;
            Negate = Expression != null && (((bool?)config.Element("Expression").Attribute("Negate")) ?? false);

            MustHaveAllTags = CreateTagsCondition(config.Element("MustHaveAllTags"));
            MustHaveOneTag = CreateTagsCondition(config.Element("MustHaveOneTag"));
        }

        private static HashSet<string> CreateTagsCondition(XElement container) {
            return container != null ? new HashSet<string>(container.Elements("Tag").Select(t => t.Value)) : new HashSet<string>();
        }
    }
}