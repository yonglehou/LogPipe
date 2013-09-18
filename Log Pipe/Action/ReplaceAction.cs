using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Consortio.Services.LogPipe.Conditions;
using Consortio.Services.LogPipe.Configuration.Action;

namespace Consortio.Services.LogPipe.Action {
    public class ReplaceAction : IAction {
        private readonly ReplaceActionConfiguration configuration;
        private readonly IMatchValueExtractor matchValueExtractor;
        private readonly Regex find;
        private readonly Regex replaceWith;

        public ReplaceAction(ReplaceActionConfiguration configuration, IExpressionBuilder expressionBuilder, IMatchValueExtractor matchValueExtractor)
        {
            this.configuration = configuration;
            this.matchValueExtractor = matchValueExtractor;

            find = expressionBuilder.Build(configuration.Expression);
            replaceWith = new Regex("\\${(?<name>.*?)}", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        }

        public void Process(IEvent evnt, IEnumerable<KeyValuePair<string, string>> extractedGroups) {
            var originalValue = matchValueExtractor.GetValue(evnt, configuration.Match);
            var newValue = find.Replace(originalValue, match => ReplaceOneMatch(evnt, match));
            matchValueExtractor.SetValue(evnt, configuration.Match, newValue);
        }

        private string ReplaceOneMatch(IEvent evnt, Match match) {
            var groups = ExtractGroups(match.Groups).ToDictionary(m => m.Key.ToLower(), m => m.Value);
            return replaceWith.Replace(configuration.With, m => {
                var value = m.Groups["name"].Value.ToLower();

                if (value.StartsWith("field.")) {
                    var fieldName = value.Substring(6).ToLower();
                    return evnt.Fields[fieldName];
                }

                if (value.StartsWith("match.")) {
                    string group;
                    var groupName = value.Substring(6).ToLower();
                    if (groups.TryGetValue(groupName, out @group)) {
                        return @group;
                    }

                    return "<missing replace group: '" + groupName + "'>";
                }

                if (value.StartsWith("match")) {
                    return groups["0"];
                }

                return "<invalid replace value: '" + value + "'>";
            });
        }

        private IEnumerable<KeyValuePair<string, string>> ExtractGroups(GroupCollection groups)
        {
            foreach (var groupName in find.GetGroupNames())
            {
                var currentGroup = groups[groupName];
                if (currentGroup.Success)
                    yield return new KeyValuePair<string, string>(groupName, currentGroup.Value);
            }
        }
    }
}