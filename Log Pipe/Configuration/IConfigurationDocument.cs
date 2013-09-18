using System.Xml.Linq;

namespace Consortio.Services.LogPipe.Configuration {
    public interface IConfigurationDocument {
        XElement Root { get; }
    }
}