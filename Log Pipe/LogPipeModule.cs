using Autofac;
using Consortio.Services.LogPipe.Action;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration;
using Consortio.Services.LogPipe.Configuration.Action;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Configuration.Input;
using Consortio.Services.LogPipe.Configuration.Output;
using Consortio.Services.LogPipe.Filter;
using Consortio.Services.LogPipe.Input;
using Consortio.Services.LogPipe.Output;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe {
    public class LogPipeModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<Configuration.Configuration>().SingleInstance();

            builder.RegisterType<LogPipe>().SingleInstance();
            builder.RegisterType<LogPipeFactory>().As<ILogPipeFactory>().SingleInstance();
            builder.RegisterType<ConfigurationFactory>().SingleInstance();

            RegisterInputs(builder);
            RegisterFilters(builder);
            RegisterActions(builder);
            RegisterOutputs(builder);
        }

        private static void RegisterInputs(ContainerBuilder builder) {
            builder.RegisterType<DirectoryPipeline>().Keyed<IPipeline>(typeof(FileInputConfiguration));
            
            builder.RegisterType<InputConfiguration>();
            builder.RegisterType<FileInputConfiguration>().Named<IInputConfiguration>("File");
            builder.RegisterType<FileState>().OnActivating(c => c.Instance.Initialize()).SingleInstance();
        }

        private static void RegisterOutputs(ContainerBuilder builder) {
            builder.RegisterType<FileOutputConfiguration>().Named<IOutputConfiguration>("File");
            builder.RegisterType<ConsoleOutputConfiguration>().Named<IOutputConfiguration>("Console");
            builder.RegisterType<ElasticSearchOutputConfiguration>().Named<IOutputConfiguration>("ElasticSearch");

            builder.RegisterType<FileOutputStream>().OnActivating(a => a.Instance.Initialize()).Keyed<IOutputStream>(typeof (FileOutputConfiguration));
            builder.RegisterType<ConsoleOutputStream>().Keyed<IOutputStream>(typeof (ConsoleOutputConfiguration));
            builder.RegisterType<ElasticSearchOutputStream>().OnActivating(a => a.Instance.Initialize()).Keyed<IOutputStream>(typeof (ElasticSearchOutputConfiguration));
        }

        private static void RegisterFilters(ContainerBuilder builder) {
            builder.RegisterType<ExpressionBuilder>().As<IExpressionBuilder>().SingleInstance();
            builder.RegisterType<MatchValueExtractor>().As<IMatchValueExtractor>().SingleInstance();
            builder.RegisterType<ConditionsMatcher>().As<IConditionsMatcher>();

            builder.RegisterType<FilterConfiguration>();

            builder.RegisterType<DropFilterConfiguration>().Named<IFilterConfiguration>("DropFilter");
            builder.RegisterType<DropFilter>().Keyed<IFilter>(typeof(DropFilterConfiguration));

            builder.RegisterType<MatchFilterConfiguration>().Named<IFilterConfiguration>("MatchFilter");
            builder.RegisterType<MatchFilter>().Keyed<IFilter>(typeof(MatchFilterConfiguration));

            builder.RegisterType<MultiLineFilterConfiguration>().Named<IFilterConfiguration>("MultiLineFilter");
            builder.RegisterType<MultiLineFilter>().Keyed<IFilter>(typeof (MultiLineFilterConfiguration));

            builder.RegisterType<ExtractTimestampFilterConfiguration>().Named<IFilterConfiguration>("ExtractTimestampFilter");
            builder.RegisterType<ExtractTimestampFilter>().Keyed<IFilter>(typeof(ExtractTimestampFilterConfiguration));

            builder.RegisterType<ConditionsConfiguration>();
        }

        private static void RegisterActions(ContainerBuilder builder) {
            builder.RegisterType<AddTagActionConfiguration>().Named<IActionConfiguration>("AddTag");
            builder.RegisterType<AddTagAction>().Keyed<IAction>(typeof(AddTagActionConfiguration));

            builder.RegisterType<RemoveTagActionConfiguration>().Named<IActionConfiguration>("RemoveTag");
            builder.RegisterType<RemoveTagAction>().Keyed<IAction>(typeof (RemoveTagActionConfiguration));

            builder.RegisterType<AddFieldActionConfiguration>().Named<IActionConfiguration>("AddField");
            builder.RegisterType<AddFieldAction>().Keyed<IAction>(typeof(AddFieldActionConfiguration));

            builder.RegisterType<RemoveFieldActionConfiguration>().Named<IActionConfiguration>("RemoveField");
            builder.RegisterType<RemoveFieldAction>().Keyed<IAction>(typeof(RemoveFieldActionConfiguration));

            builder.RegisterType<ExtractFieldsActionConfiguration>().Named<IActionConfiguration>("ExtractFields");
            builder.RegisterType<ExtractFieldsAction>().Keyed<IAction>(typeof(ExtractFieldsActionConfiguration));

            builder.RegisterType<RemoveActionConfiguration>().Named<IActionConfiguration>("Remove");
            builder.RegisterType<RemoveAction>().Keyed<IAction>(typeof(RemoveActionConfiguration));

            builder.RegisterType<ReplaceActionConfiguration>().Named<IActionConfiguration>("Replace");
            builder.RegisterType<ReplaceAction>().Keyed<IAction>(typeof(ReplaceActionConfiguration));
        }
    }
}