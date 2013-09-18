using System;
using System.Configuration;
using System.Xml.Linq;
using Common.Logging;
using Consortio.Services.LogPipe.Configuration;

namespace Consortio.LogPipe.Host {
    internal class AppConfigConfigurationDocument : IConfigurationDocument {
        private readonly ILog logger = LogManager.GetLogger<AppConfigConfigurationDocument>();

        public XElement Root { get; private set; }

        public void Load() {
            try {
                Root = ((XDocument)ConfigurationManager.GetSection("LogPipe")).Root;
            } catch (Exception ex) {
                logger.Error("Failed to load config", ex);
                throw;
            }
        }
    }
}