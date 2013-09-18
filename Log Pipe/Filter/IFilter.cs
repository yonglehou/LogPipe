using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Filter {
    public interface IFilter {
        FilterFlow Process(ref IEvent evnt);
    }
}