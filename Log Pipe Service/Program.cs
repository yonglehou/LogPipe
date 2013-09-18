using System;
using System.IO;
using Autofac;
using Common.Logging;
using Consortio.LogPipe.Host.Logging;
using Consortio.Services.LogPipe;
using Consortio.Services.LogPipe.Configuration;
using Topshelf;

namespace Consortio.LogPipe.Host {
    internal class Program {
        private static void Main(string[] args) {
            ILog logger = LogManager.GetLogger<Program>();

            try {
                logger.Info("Starting");

                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                IContainer container = InitializeContainer();

                HostFactory.Run(x => {
                    x.Service<Services.LogPipe.LogPipe>(s => {
                        s.ConstructUsing(name => container.Resolve<Services.LogPipe.LogPipe>());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });

                    x.UseLog4Net();

                    x.RunAsLocalSystem();
                    x.SetDescription("Processes log files");
                    x.SetDisplayName("LogPipe");
                    x.SetServiceName("LogPipe");
                });

                logger.Info("Started");
            } catch (Exception ex) {
                logger.Error("Failed to start", ex);
            }
        }

        private static IContainer InitializeContainer() {
            var configuration = LoadConfiguration();

            var builder = new ContainerBuilder();
            builder.RegisterModule<LogPipeModule>();
            builder.RegisterInstance(configuration);
            IContainer container = builder.Build();
            return container;
        }

        private static IConfigurationDocument LoadConfiguration() {
            var configuration = new AppConfigConfigurationDocument();
            configuration.Load();
            return configuration;
        }
    }
}