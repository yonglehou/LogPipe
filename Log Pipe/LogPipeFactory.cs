using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Consortio.Services.LogPipe.Action;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration;
using Consortio.Services.LogPipe.Configuration.Action;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Configuration.Output;
using Consortio.Services.LogPipe.Filter;
using Consortio.Services.LogPipe.Output;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe {
    internal class LogPipeFactory : ILogPipeFactory {
        private readonly Configuration.Configuration configuration;
        private readonly ILifetimeScope lifetimeScope;

        public LogPipeFactory(Configuration.Configuration configuration, ILifetimeScope lifetimeScope) {
            this.configuration = configuration;
            this.lifetimeScope = lifetimeScope;
        }

        public IConditionsMatcher CreateConditionMatcher(ConditionsConfiguration conditions) {
            return lifetimeScope.Resolve<IConditionsMatcher>(new Parameter[] {
                new TypedParameter(typeof(ConditionsConfiguration), conditions)
            });
        }

        public IEnumerable<IFilter> CreateFilters(string type) {
            return CreateFromConfiguration<IFilterConfiguration, IFilter>(configuration.Filter.Filters, type);
        }

        public IEnumerable<IAction> CreateActions(IEnumerable<IActionConfiguration> configurations) {
            foreach (var item in configurations) {
                Type configurationType = item.GetType();
                yield return lifetimeScope.ResolveKeyed<IAction>(
                    configurationType,
                    new Parameter[] {
                        new TypedParameter(configurationType, item)
                    });
            }
        }

        public IEnumerable<IPipeline> CreateProcessors() {
            List<IOutputStream> outputs = CreateOutputs().ToList();

            foreach (var item in configuration.Input.Inputs) {
                Type configurationType = item.GetType();
                IEnumerable<IOutputStream> matchingOutputs = outputs.Where(o => string.IsNullOrWhiteSpace(o.Type) || o.Type == item.Type);
                yield return lifetimeScope.ResolveKeyed<IPipeline>(
                    configurationType,
                    new Parameter[] {
                        new TypedParameter(configurationType, item),
                        new TypedParameter(typeof (IEnumerable<IOutputStream>), matchingOutputs)
                    });
            }
        }

        private IEnumerable<IOutputStream> CreateOutputs() {
            return CreateFromConfiguration<IOutputConfiguration, IOutputStream>(configuration.Output.Outputs);
        }

        private IEnumerable<TResult> CreateFromConfiguration<TConfig, TResult>(IEnumerable<TConfig> configs, string type = null) where TConfig : ITypeConfiguration {
            foreach (TConfig item in configs.Where(c => string.IsNullOrWhiteSpace(c.Type) || c.Type == type)) {
                Type configurationType = item.GetType();
                yield return lifetimeScope.ResolveKeyed<TResult>(configurationType, new Parameter[] {new TypedParameter(configurationType, item)});
            }
        }
    }
}