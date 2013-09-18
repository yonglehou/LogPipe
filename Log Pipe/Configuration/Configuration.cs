using Autofac;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Configuration.Input;
using Consortio.Services.LogPipe.Configuration.Output;
using Consortio.Services.LogPipe.Configuration.Patterns;

namespace Consortio.Services.LogPipe.Configuration {
    public class Configuration {
        public Configuration(IConfigurationDocument document, ILifetimeScope container) {
            Input = new InputConfiguration(document.Root.Element("Input"), container);
            Filter = new FilterConfiguration(document.Root.Element("Filter"), container);
            Output = new OutputConfiguration(document.Root.Element("Output"), container);
            Patterns = new PatternsConfiguration(document.Root.Element("Patterns"));
        }

        public InputConfiguration Input { get; private set; }
        public FilterConfiguration Filter { get; private set; }
        public OutputConfiguration Output { get; private set; }
        public PatternsConfiguration Patterns { get; private set; }
    }
}