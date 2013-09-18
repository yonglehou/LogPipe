using Common.Logging;

namespace Consortio.Services.LogPipe.Conditions {
    public class MatchValueExtractor : IMatchValueExtractor {
        private readonly ILog logger;

        public MatchValueExtractor() {
            logger = LogManager.GetLogger<MatchValueExtractor>();
        }

        public string GetValue(IEvent evnt, string match) {
            if (string.IsNullOrWhiteSpace(match))
                return evnt.Message;

            match = match.ToLower();

            switch (match.ToLower()) {
                case "source":
                    return evnt.Source;
                case "message":
                    return evnt.Message;
            }

            if (match.StartsWith("field.")) {
                var fieldName = match.Substring(6).ToLower();
                string fieldValue;
                return evnt.Fields.TryGetValue(fieldName, out fieldValue) ? fieldValue : null;
            }

            logger.Error(string.Format("Match='{0}' not valid", match));

            return null;
        }

        public void SetValue(IEvent evnt, string match, string value) {
            if (string.IsNullOrWhiteSpace(match)) {
                match = "message";
            }

            match = match.ToLower();

            switch (match.ToLower()) {
                case "source":
                    evnt.Source = value;
                    return;
                case "message":
                    evnt.Message = value;
                    return;
            }

            if (match.StartsWith("field.")) {
                var fieldName = match.Substring(6).ToLower();
                evnt.Fields[fieldName] = value;
                return;
            }

            logger.Error(string.Format("Match='{0}' not valid", match));
        }
    }
}