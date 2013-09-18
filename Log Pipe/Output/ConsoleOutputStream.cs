using System;
using Consortio.Services.LogPipe.Configuration.Output;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Output {
    public class ConsoleOutputStream : IOutputStream {
        private readonly ConsoleOutputConfiguration configuration;

        public ConsoleOutputStream(ConsoleOutputConfiguration configuration) {
            this.configuration = configuration;
        }

        public string Type { get { return configuration.Type; } }

        public OutputFlow Write(IEvent evnt, PipelineContext pipelineContext) {
            Console.WriteLine(evnt.ToString());
            return OutputFlow.Successfull;
        }

        public void Dispose() {
        }
    }
}