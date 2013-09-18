using System.Text.RegularExpressions;
using Consortio.Services.LogPipe.Configuration.Patterns;

namespace Consortio.Services.LogPipe.Conditions {
    public class ExpressionBuilder : IExpressionBuilder {
        private readonly Regex expressionRegex = new Regex(@"%{(?<name>[0-9A-Za-z_]+?)(:(?<field>[0-9A-Za-z_]+?))?}", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private readonly PatternsConfiguration patterns;

        public ExpressionBuilder(Configuration.Configuration configuration) {
            patterns = configuration.Patterns;
        }

        public Regex Build(string input) {
            if (input == null)
                return null;

            var result = input;
            while (expressionRegex.IsMatch(result)) {
                result = expressionRegex.Replace(result, Evaluator);
            }

            return new Regex(result, RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        }

        private string Evaluator(Match match) {
            var nameGroup = match.Groups["name"];
            var fieldGroup = match.Groups["field"];
            if (fieldGroup.Success) {
                return "(?<" + fieldGroup.Value + ">" + patterns[nameGroup.Value] + ")";    
            }

            return patterns[nameGroup.Value];
        }
    }
}