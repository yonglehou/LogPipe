using System;
using System.IO;
using Topshelf.Logging;
using log4net;
using log4net.Config;

namespace Consortio.LogPipe.Host.Logging {
    public class Log4NetLogWriterFactory :
        LogWriterFactory
    {
        public LogWriter Get(string name)
        {
            return new Log4NetLogWriter(LogManager.GetLogger(name));
        }

        public void Shutdown()
        {
            LogManager.Shutdown();
        }

        public static void Use()
        {
            HostLogger.UseLogger(new Log4NetLoggerConfigurator(null));
        }

        public static void Use(string file)
        {
            HostLogger.UseLogger(new Log4NetLoggerConfigurator(file));
        }

        [Serializable]
        public class Log4NetLoggerConfigurator :
            HostLoggerConfigurator
        {
            readonly string _file;

            public Log4NetLoggerConfigurator(string file)
            {
                _file = file;
            }

            public LogWriterFactory CreateLogWriterFactory()
            {
                if (!string.IsNullOrEmpty(_file))
                {
                    string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _file);
                    var configFile = new FileInfo(file);
                    if (configFile.Exists)
                    {
                        XmlConfigurator.Configure(configFile);
                    }
                }

                return new Log4NetLogWriterFactory();
            }
        }
    }
}