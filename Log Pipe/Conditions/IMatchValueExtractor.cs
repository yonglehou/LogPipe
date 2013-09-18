namespace Consortio.Services.LogPipe.Conditions {
    public interface IMatchValueExtractor {
        string GetValue(IEvent evnt, string match);
        void SetValue(IEvent evnt, string match, string value);
    }
}