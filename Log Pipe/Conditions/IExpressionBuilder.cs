using System.Text.RegularExpressions;

namespace Consortio.Services.LogPipe.Conditions {
    public interface IExpressionBuilder {
        Regex Build(string input);
    }
}