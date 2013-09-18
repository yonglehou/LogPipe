using Topshelf.HostConfigurators;

namespace Consortio.LogPipe.Host.Logging {
    /// <summary>
    ///   Extensions for configuring Logging for log4net
    /// </summary>
    public static class Log4NetConfigurationExtensions
    {
        /// <summary>
        ///   Specify that you want to use the Log4net logging engine.
        /// </summary>
        /// <param name="configurator"> </param>
        public static void UseLog4Net(this HostConfigurator configurator)
        {
            Log4NetLogWriterFactory.Use();
        }

        /// <summary>
        ///   Specify that you want to use the Log4net logging engine.
        /// </summary>
        /// <param name="configurator"> </param>
        /// <param name="configFileName"> The name of the log4net xml configuration file </param>
        public static void UseLog4Net(this HostConfigurator configurator, string configFileName)
        {
            Log4NetLogWriterFactory.Use(configFileName);
        }
    }
}