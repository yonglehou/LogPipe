using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Consortio.LogPipe.Host.Configuration {
    public class LogPipeSection : IConfigurationSectionHandler {
        public object Create(object parent, object configContext, XmlNode section) {
            return XDocument.Load(new XmlNodeReader(section));
        }
    }
}
