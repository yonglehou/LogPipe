using System;
using System.Collections.Generic;
using System.Threading;
using Common.Logging;
using Consortio.Services.LogPipe.Configuration.Output;
using Consortio.Services.LogPipe.Pipeline;
using Nest;

namespace Consortio.Services.LogPipe.Output {
    public class ElasticSearchOutputStream : IOutputStream {
        private readonly ElasticSearchOutputConfiguration configuration;

        private readonly HashSet<string> indexNames = new HashSet<string>();
        private readonly ILog logger;

        private readonly Semaphore pool;
        private ElasticClient client;

        public ElasticSearchOutputStream(ElasticSearchOutputConfiguration configuration) {
            this.configuration = configuration;
            pool = new Semaphore(configuration.ConnectionLimit, configuration.ConnectionLimit);
            logger = LogManager.GetCurrentClassLogger();
        }

        public string Type {
            get { return configuration.Type; }
        }

        public OutputFlow Write(IEvent evnt, PipelineContext pipelineContext) {
            while (true) {
                try {
                    if (pipelineContext.CancelationRequested)
                        return OutputFlow.Failed;

                    if (!pool.WaitOne(TimeSpan.FromSeconds(1))) {
                        if (pipelineContext.CancelationRequested)
                            return OutputFlow.Failed;

                        continue;
                    }

                    string indexName = BuildIndexName(evnt);

                    OutputFlow flow = EnsureIndexExists(indexName, pipelineContext);
                    if (flow != OutputFlow.Successfull)
                        return flow;

                    flow = Index(evnt, indexName, pipelineContext);

                    return flow;
                } finally {
                    pool.Release();
                }
            }
        }

        public void Dispose() {}

        public void Initialize() {
            var clientSettings = new ConnectionSettings(new Uri(string.Format("http://{0}:{1}", configuration.Host, configuration.Port)));
            client = new ElasticClient(clientSettings);
        }

        private OutputFlow EnsureIndexExists(string indexName, PipelineContext pipelineContext) {
            if (indexNames.Contains(indexName))
                return OutputFlow.Successfull;

            while (true) {
                if (pipelineContext.CancelationRequested)
                    return OutputFlow.Failed;

                if (CreateIndex(indexName)) {
                    indexNames.Add(indexName);
                    return OutputFlow.Successfull;
                }

                Thread.Sleep(10000);
            }
        }

        private OutputFlow Index(IEvent evnt, string indexName, PipelineContext pipelineContext) {
            while (true) {
                if (pipelineContext.CancelationRequested)
                    return OutputFlow.Failed;

                if (Index(evnt, indexName))
                    return OutputFlow.Successfull;

                Thread.Sleep(10000);
            }
        }

        private bool CreateIndex(string indexName) {
            if (client.IndexExists(indexName).Exists)
                return true;
            
            var indexSettings = new IndexSettings();
            indexSettings.Add("index.store.compress.stored", true);
            indexSettings.Add("index.store.compress.tv", true);
            indexSettings.Add("index.query.default_field", "@message");
            IIndicesOperationResponse result = client.CreateIndex(indexName, indexSettings);

            CreateMappings(indexName);

            if (!result.OK) {
                logger.Error(string.Format("Failed to create index: '{0}'. Result: '{1}' Retrying...", indexName, result.ConnectionStatus.Result));
            }

            return result.OK;
        }

        private void CreateMappings(string indexName) {
            client.MapFluent(map => map
                .IndexName(indexName)
                .DisableAllField()
                .TypeName("_default_")
                .TtlField(t => t.SetDisabled(false))
                .SourceField(s => s.SetCompression(true))
                .DynamicTemplates(descriptor => descriptor
                    .Add(t =>
                        t.Name("fields_template")
                            .Mapping(m => m.Generic(g => g.Index("not_analyzed").Type("string")))
                            .PathMatch("@fields.*")
                    )
                    .Add(t =>
                        t.Name("tags_template")
                            .Mapping(m => m.Generic(g => g.Index("not_analyzed").Type("string")))
                            .PathMatch("@tags.*")
                    )
                )
                .Properties(descriptor => descriptor
                    .Object<dynamic>(m => m.Name("@fields").Dynamic().Path("full"))
                    .String(m => m.Name("@source").Index(FieldIndexOption.not_analyzed))
                    .Date(m => m.Name("@timestamp").Index(NonStringIndexOption.not_analyzed))
                    .String(m => m.Name("@type").Index(FieldIndexOption.not_analyzed))
                    .String(m => m.Name("@message").IndexAnalyzer("whitespace"))
                )
            );
        }

        private bool Index(IEvent evnt, string indexName) {
            string ttl = configuration.TTL[evnt.Type];
            var logStashEvent = new LogStashEvent(evnt) {TTL = ttl};

            IIndexResponse result = client.Index(logStashEvent, indexName, evnt.Type);
            if (!result.OK) {
                logger.Error(string.Format("Failed to index: '{0}'. Result: '{1}'. Retrying...", evnt, result.ConnectionStatus.Result));
            }

            return result.OK;
        }

        private string BuildIndexName(IEvent evnt) {
            return evnt.Timestamp.ToString(configuration.IndexNameFormat);
        }
    }
}