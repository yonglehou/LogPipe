using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using Autofac.Core;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    public class FilterConfiguration {
        public FilterConfiguration(XElement element, ILifetimeScope container) {
            Filters = element.Elements()
                .Select(e =>
                    container.ResolveNamed<IFilterConfiguration>(
                    e.Name.ToString(),
                    new Parameter[] { new TypedParameter(typeof(XElement), e) })
                ).ToList();
        }

        public IEnumerable<IFilterConfiguration> Filters { get; set; }
    }
}