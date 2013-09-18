using System.Collections.Generic;
using Common.Logging;
using Consortio.Services.LogPipe.Filter;
using Consortio.Services.LogPipe.Output;

namespace Consortio.Services.LogPipe.Pipeline {
    public abstract class PipelineBase : IPipeline {
        private readonly PipelineContext pipelineContext;
        private readonly IEnumerable<IFilter> filters;
        private readonly IEnumerable<IOutputStream> outputs;

        private readonly ILog logger;

        protected PipelineBase(PipelineContext pipelineContext, IEnumerable<IFilter> filters, IEnumerable<IOutputStream> outputs) {
            this.pipelineContext = pipelineContext;
            this.filters = filters;
            this.outputs = outputs;
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Start() {
        }

        public virtual void Stop() {
            pipelineContext.CancelationRequested = true;
        }

        protected PipelineFlow RunPipeline(IEvent evnt) {
            logger.Trace("Read line: " + evnt);

            var result = FilterFlow.Continue;
            foreach (var filter in filters) {
                result = filter.Process(ref evnt);
                if (result != FilterFlow.Continue)
                    break;
            }

            logger.Trace("PipelineFlow: " + result);

            if (result == FilterFlow.Drop)
                return PipelineFlow.Drop;

            if(result == FilterFlow.Partial)
                return PipelineFlow.Partial;

            logger.Trace("Writing: " + evnt);

            var flow = WriteToOutput(evnt);
            return flow == OutputFlow.Failed ? PipelineFlow.Failed : PipelineFlow.Completed;
        }

        public abstract void Process();

        private OutputFlow WriteToOutput(IEvent evnt)
        {
            var allSuccessfull = true;
            foreach (var output in outputs) {
                var flow = output.Write(evnt, pipelineContext);
                if (flow != OutputFlow.Successfull)
                    allSuccessfull = false;
            }

            return allSuccessfull ? OutputFlow.Successfull : OutputFlow.Failed;
        }
    }
}