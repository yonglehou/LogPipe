using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Common.Logging;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration.Filter;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Filter {
    public class ExtractTimestampFilter : IFilter {
        private const string EventTimeKey = "event_time";
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ssK";
        private static readonly Regex regex = new Regex(@"\b(\d{4})-(0[1-9]|1[0-2])-([12]\d|0[1-9]|3[01])\s([01]\d|2[0-3]):([0-5]\d):([0-5]\d)Z?\b", RegexOptions.Compiled);
        private readonly IConditionsMatcher conditionsMatcher;
        private readonly ExtractTimestampFilterConfiguration configuration;
        private readonly ILog logger;

        public ExtractTimestampFilter(ExtractTimestampFilterConfiguration configuration, ILogPipeFactory logPipeFactory) {
            this.configuration = configuration;
            conditionsMatcher = logPipeFactory.CreateConditionMatcher(configuration.Conditions);
            logger = LogManager.GetCurrentClassLogger();
        }

        public FilterFlow Process(ref IEvent evnt) {
            try {
                var result = conditionsMatcher.Process(evnt);
                if (!result.IsMatch)
                    return FilterFlow.Continue;

                Match match = regex.Match(evnt.Message);
                if (match.Success) {
                    evnt.Fields[EventTimeKey] = match.Value;
                    evnt.Timestamp = DateTime.ParseExact(match.Value, DateTimeFormat, CultureInfo.InvariantCulture, configuration.DateTimeStyles);
                }
            } catch (Exception ex) {
                logger.Error(evnt, ex);
            }

            return FilterFlow.Continue;
        }
    }
}