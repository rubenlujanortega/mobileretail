using Serilog;
using System;
using System.Configuration;

namespace ClientProducts.Logs
{
    public class SeriLogHelper : ISeriLogHelper
    {
        private readonly ILogger iLogger;

        public SeriLogHelper()
        {
            iLogger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(new Serilog.Core.LoggingLevelSwitch((Serilog.Events.LogEventLevel)int.Parse(ConfigurationManager.AppSettings["SeriLog:LogEventLevel"])))
                .WriteTo.File(ConfigurationManager.AppSettings["SeriLog:FileName"], rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
        }

        public void Trace(string messageTemplate) { iLogger.Verbose(messageTemplate); }
        public void Trace(string messageTemplate, params object[] propertyValues) { iLogger.Verbose(messageTemplate, propertyValues); }

        public void Debug(string messageTemplate) { iLogger.Debug(messageTemplate); }
        public void Debug(string messageTemplate, params object[] propertyValues) { iLogger.Debug(messageTemplate, propertyValues); }

        public void Info(string messageTemplate) { iLogger.Information(messageTemplate); }
        public void Info(string messageTemplate, params object[] propertyValues) { iLogger.Information(messageTemplate, propertyValues); }

        public void Warn(string messageTemplate) { iLogger.Warning(messageTemplate); }
        public void Warn(string messageTemplate, params object[] propertyValues) { iLogger.Warning(messageTemplate, propertyValues); }

        public void Error(Exception exception, string messageTemplate) { iLogger.Error(exception, messageTemplate); }
        public void Error(Exception exception, string messageTemplate, params object[] propertyValues) { iLogger.Error(exception, messageTemplate, propertyValues); }

        public void Fatal(Exception exception, string messageTemplate) { iLogger.Fatal(exception, messageTemplate); }
        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues) { iLogger.Fatal(exception, messageTemplate, propertyValues); }
    }
}
