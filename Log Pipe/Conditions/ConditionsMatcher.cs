using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Consortio.Services.LogPipe.Configuration.Filter;

namespace Consortio.Services.LogPipe.Conditions {
    public class ConditionsMatcher : IConditionsMatcher {
        private readonly Regex expression;
        private readonly IMatchValueExtractor matchValueExtractor;

        private readonly ConditionsConfiguration conditionsConfiguration;

        public ConditionsMatcher(ConditionsConfiguration conditionsConfiguration, IExpressionBuilder expressionBuilder, IMatchValueExtractor matchValueExtractor) {
            this.conditionsConfiguration = conditionsConfiguration;
            this.matchValueExtractor = matchValueExtractor;
            expression = expressionBuilder.Build(conditionsConfiguration.Expression);
        }

        public IConditionsMatcherResult Process(IEvent evnt) {
            var result = new ConditionsMatcherResult { IsMatch = false, EmptyValue = true, ExtractedGroups = Enumerable.Empty<KeyValuePair<string, string>>()};

            if (expression != null) {
                var value = matchValueExtractor.GetValue(evnt, conditionsConfiguration.Match);
                if (string.IsNullOrWhiteSpace(value))
                    return result;

                result.EmptyValue = false;

                var match = expression.Match(value);
                if (!match.Success && !conditionsConfiguration.Negate)
                    return result;

                result.ExtractedGroups = ExtractGroups(match.Groups).ToList();
            }

            var tagsMatching = AllMatching(evnt, conditionsConfiguration.MustHaveAllTags) && AtLeastOneMatching(evnt, conditionsConfiguration.MustHaveOneTag);
            if (!tagsMatching)
                return result;

            result.IsMatch = true;
            return result;
        }

        private IEnumerable<KeyValuePair<string, string>> ExtractGroups(GroupCollection groups) {
            foreach (var groupName in expression.GetGroupNames().Skip(1)) {
                var currentGroup = groups[groupName];
                if (currentGroup.Success)
                    yield return new KeyValuePair<string, string>(groupName, currentGroup.Value);
            }
        }

        private bool AtLeastOneMatching(IEvent evnt, IEnumerable<string> tags) {
            if (!tags.Any())
                return true;

            return tags.Any(t => evnt.Tags.Contains(t));
        }

        private bool AllMatching(IEvent evnt, IEnumerable<string> tags) {
            if (!tags.Any())
                return true;

            return tags.All(t => evnt.Tags.Contains(t));
        }
    }
}