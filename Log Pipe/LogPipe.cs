using System.Collections.Generic;
using Common.Logging;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe {
    public class LogPipe {
        private readonly ILogPipeFactory logPipeFactory;
        private readonly List<IPipeline> processors = new List<IPipeline>();
        private readonly ILog logger;

        public LogPipe(ILogPipeFactory logPipeFactory) {
            this.logPipeFactory = logPipeFactory;
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Start() {
            logger.Debug("Starting processors");
            foreach (var processor in logPipeFactory.CreateProcessors()) {
                processors.Add(processor);
                processor.Start();
            }
            logger.Debug("Started processors");
        }

        public void Stop() {
            logger.Debug("Stopping processors");
            foreach (var processor in processors) {
                processor.Stop();
            }
            logger.Debug("Stopped processors");
        }
    }
}