using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using Autofac.Core;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Configuration.Filter {
    public class ActionsConfiguration : IEnumerable<IActionConfiguration> {
        private readonly List<IActionConfiguration> actions;

        public ActionsConfiguration(XElement element, ILifetimeScope container) {
            if (element == null) {
                actions = new List<IActionConfiguration>();
            } else {
                actions = element.Elements()
                    .Select(e =>
                        container.ResolveNamed<IActionConfiguration>(
                            e.Name.ToString(),
                            new Parameter[] {new TypedParameter(typeof (XElement), e)})
                    ).ToList();
            }
        }

        public IEnumerator<IActionConfiguration> GetEnumerator() {
            return actions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}