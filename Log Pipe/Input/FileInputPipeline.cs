using System;
using System.Collections.Generic;
using System.IO;
using Common.Logging;
using Consortio.Services.LogPipe.Configuration.Input;
using Consortio.Services.LogPipe.Filter;
using Consortio.Services.LogPipe.Output;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Input {
    public class FileInputPipeline : PipelineBase {
        private readonly FileInputConfiguration configuration;
        private readonly FileState fileState;
        private readonly ILog logger;
        private readonly FileInfo path;
        private readonly PipelineContext pipelineContext;

        private long lastReadPosition;

        public FileInputPipeline(PipelineContext pipelineContext, FileInputConfiguration configuration, FileInfo path, FileState fileState, IEnumerable<IFilter> filters, IEnumerable<IOutputStream> outputs) : base(pipelineContext, filters, outputs) {
            this.configuration = configuration;
            this.path = path;
            this.fileState = fileState;
            this.pipelineContext = pipelineContext;

            lastReadPosition = fileState[path.FullName, configuration.Type];

            logger = LogManager.GetCurrentClassLogger();
        }

        public override void Process() {
            logger.Trace("Started processing file: " + path.FullName);

            bool partial = false;
            path.Refresh();

            if (!path.Exists) {
                logger.Trace("Stopped processing file: " + path.FullName + ". Deleted.");
                return;
            }

            if (path.Length == lastReadPosition) {
                logger.Trace("Stopped processing file: " + path.FullName + ". Nothing new.");
                return;
            }

            try {
                using (var fr = new FileReader(path.FullName, lastReadPosition, configuration.Encoding)) {
                    string line;
                    while (!pipelineContext.CancelationRequested && (line = fr.ReadLine()) != null) {
                        var flow = PipelineFlow.Completed;
                        if (!string.IsNullOrWhiteSpace(line))
                            flow = RunPipeline(CreateEventFromRaw(line, fr.Position));

                        if (flow == PipelineFlow.Failed)
                            break;

                        if (flow == PipelineFlow.Partial)
                            partial = true;

                        if (flow == PipelineFlow.Completed) {
                            fileState[path.FullName, configuration.Type] = partial ? lastReadPosition : fr.Position;
                            partial = false;
                        }

                        lastReadPosition = fr.Position;

                        if (pipelineContext.CancelationRequested)
                            break;
                    }
                }
            } catch (Exception ex) {
                logger.Error(string.Format("Failed to process input file. File: '{0}'", path), ex);
            }

            logger.Trace("Stopped processing file: " + path.FullName);
        }

        private IEvent CreateEventFromRaw(string rawLine, long position) {
            var evnt = new Event(rawLine, configuration.Type, CreateSource(), DateTime.UtcNow, configuration.Tags);
            evnt.Sequence = (evnt.Source + configuration.Type + position).GetHashCode().ToString("X");
            return evnt;
        }

        private string CreateSource() {
            return string.Format("file://{0}/{1}", Environment.MachineName, path.FullName.Replace("\\", "/"));
        }
    }
}