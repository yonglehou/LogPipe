using System;
using System.IO;
using Common.Logging;
using Consortio.Services.LogPipe.Configuration.Output;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Output {
    public class FileOutputStream : IOutputStream {
        private readonly FileOutputConfiguration configuration;
        private FileStream fileStream;
        private StreamWriter streamWriter;
        private readonly ILog logger;

        public FileOutputStream(FileOutputConfiguration configuration) {
            this.configuration = configuration;
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Initialize() {
            fileStream = new FileStream(configuration.Path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            streamWriter = new StreamWriter(fileStream);
        }

        public string Type {
            get { return configuration.Type; }
        }

        public OutputFlow Write(IEvent evnt, PipelineContext pipelineContext) {
            try {
                streamWriter.WriteLine(evnt.ToString());
                return OutputFlow.Successfull;
            } catch (Exception ex) {
                logger.Error(string.Format("Failure while writting to output file. Path: '{0}'", configuration.Path), ex);
                return OutputFlow.Failed;
            }
        }

        public void Dispose() {
            streamWriter.Dispose();
            fileStream.Dispose();
        }
    }
}