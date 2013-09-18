using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using Autofac.Core;

namespace Consortio.Services.LogPipe.Configuration.Input {
    public class InputConfiguration {
        public InputConfiguration(XElement element, ILifetimeScope container) {
            Inputs = element.Elements()
                .Select(e =>
                    container.ResolveNamed<IInputConfiguration>(
                    e.Name.ToString(),
                    new Parameter[] { new TypedParameter(typeof(XElement), e) })
                ).ToList();
        }

        public IEnumerable<IInputConfiguration> Inputs { get; private set; }
    }
}