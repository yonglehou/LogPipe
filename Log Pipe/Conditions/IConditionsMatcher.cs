using System.Collections.Generic;

namespace Consortio.Services.LogPipe.Conditions {
    public interface IConditionsMatcher {
        IConditionsMatcherResult Process(IEvent evnt);
    }

    public interface IConditionsMatcherResult {
        bool IsMatch { get; set; }
        IEnumerable<KeyValuePair<string, string>> ExtractedGroups { get; set; }
        bool EmptyValue { get; set; }
    }

    public class ConditionsMatcherResult : IConditionsMatcherResult {
        public bool IsMatch { get; set; }
        public IEnumerable<KeyValuePair<string, string>> ExtractedGroups { get; set; }
        public bool EmptyValue { get; set; }
    }
}