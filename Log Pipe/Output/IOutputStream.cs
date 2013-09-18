using System;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Output {
    public interface IOutputStream : IDisposable {
        string Type { get; }
        OutputFlow Write(IEvent evnt, PipelineContext pipelineContext);
    }
}