using System.Text.RegularExpressions;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Filter {
    internal class MultiLineFilter : IFilter {
        private readonly Regex expression;
        private IEvent aggregatedEvent;

        public MultiLineFilter(MultiLineFilterConfiguration configuration, IExpressionBuilder expressionBuilder) {
            expression = expressionBuilder.Build(configuration.Conditions.Expression);
        }

        public FilterFlow Process(ref IEvent evnt) {
            if (aggregatedEvent != null && expression.IsMatch(evnt.Message)) {
                IEvent newEvnt = evnt;
                evnt = aggregatedEvent;
                aggregatedEvent = newEvnt;
                return FilterFlow.Continue;
            }

            if (aggregatedEvent == null)
                aggregatedEvent = evnt;
            else
                MergeWithAggregated(evnt);

            return FilterFlow.Partial;
        }

        private void MergeWithAggregated(IEvent evnt) {
            aggregatedEvent.Message = aggregatedEvent.Message + "\n" + evnt.Message;
            foreach (string tag in evnt.Tags) {
                aggregatedEvent.Tags.Add(tag);
            }

            foreach (var field in evnt.Fields) {
                aggregatedEvent.Fields[field.Key] = field.Value;
            }
        }
    }
}