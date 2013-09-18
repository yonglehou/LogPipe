using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Common.Logging;
using Consortio.Services.LogPipe.Configuration.Input;
using Consortio.Services.LogPipe.Output;
using Consortio.Services.LogPipe.Pipeline;

namespace Consortio.Services.LogPipe.Input {
    public class DirectoryPipeline : IPipeline {
        private readonly FileInputConfiguration configuration;
        private readonly string directory;
        private readonly FileState fileState;
        private readonly string filter;
        private readonly ILogPipeFactory logPipeFactory;
        private readonly ILog logger;
        private readonly IEnumerable<IOutputStream> outputs;

        private readonly ConcurrentDictionary<string, FileInputPipeline> streams = new ConcurrentDictionary<string, FileInputPipeline>();

        private readonly Timer timer;
        private readonly FileSystemWatcher watcher = new FileSystemWatcher();

        public DirectoryPipeline(FileInputConfiguration configuration, IEnumerable<IOutputStream> outputs, ILogPipeFactory logPipeFactory, FileState fileState) {
            this.configuration = configuration;
            this.outputs = outputs;
            this.logPipeFactory = logPipeFactory;

            logger = LogManager.GetCurrentClassLogger();

            directory = Path.GetDirectoryName(this.configuration.Path);
            filter = Path.GetFileName(this.configuration.Path).Trim();

            this.fileState = fileState;

            watcher.Path = directory;
            watcher.Filter = filter;
            watcher.Created += FileCreated;
            watcher.Deleted += FileDeleted;

            timer = new Timer(TimerElapssed);
        }

        public void Start() {
            foreach (string file in Directory.GetFiles(directory, filter))
                CreatePipeline(file);

            watcher.EnableRaisingEvents = true;
            timer.Change(0, (long)configuration.Interval.TotalMilliseconds);
        }

        public void Stop() {
            watcher.EnableRaisingEvents = false;

            foreach (FileInputPipeline pipeline in streams.Values)
                pipeline.Stop();

            Lock(() => {
                foreach (string pipeline in streams.Keys) {
                    DestroyPipeline(pipeline);
                }
            });
        }

        private void TimerElapssed(object state) {
            timer.Change(Timeout.Infinite, Timeout.Infinite);

            Lock(() => {
                foreach (var pipeline in streams.Values)
                    pipeline.Process();
            });

            timer.Change((long)configuration.Interval.TotalMilliseconds, (long)configuration.Interval.TotalMilliseconds);
        }

        private void Lock(System.Action action) {
            lock (this) {
                action();
            }
        }

        private void FileCreated(object sender, FileSystemEventArgs fileSystemEventArgs) {
            if (streams.ContainsKey(fileSystemEventArgs.FullPath))
                return;

            Lock(() => CreatePipeline(fileSystemEventArgs.FullPath));
        }

        private void FileDeleted(object sender, FileSystemEventArgs e) {
            string fullPath = e.FullPath;
            Lock(() => DestroyPipeline(fullPath));
        }

        private void CreatePipeline(string path) {
            var pipeline = new FileInputPipeline(
                new PipelineContext(),
                configuration,
                new FileInfo(path),
                fileState,
                logPipeFactory.CreateFilters(configuration.Type).ToList(),
                outputs
                );

            streams[path] = pipeline;

            logger.Info(string.Format("Starting input stream. Path: '{0}'", path));
        }

        private void DestroyPipeline(string fullPath) {
            logger.Info(string.Format("Stopping input stream. Path: '{0}'", fullPath));

            FileInputPipeline pipeline;
            streams.TryRemove(fullPath, out pipeline);
            fileState.Remove(fullPath, configuration.Type);
        }
    }
}