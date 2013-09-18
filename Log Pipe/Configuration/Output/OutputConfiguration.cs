using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using Autofac.Core;

namespace Consortio.Services.LogPipe.Configuration.Output {
    public class OutputConfiguration {
        public OutputConfiguration(XElement element, ILifetimeScope container) {
            Outputs = element.Elements()
                .Select(e =>
                    container.ResolveNamed<IOutputConfiguration>(
                        e.Name.ToString(),
                        new Parameter[] {new TypedParameter(typeof (XElement), e)})
                ).ToList();
        }

        public IEnumerable<IOutputConfiguration> Outputs { get; private set; }
    }
}