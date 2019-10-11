using Microsoft.Extensions.Logging;
using Nhea.Configuration;
using Nhea.Logging;
using System;

namespace Nhea.Services
{
    public class NheaLogger : ILogger
    {
        private readonly NheaLoggerProvider _provider;
        private readonly string _category;

        public NheaLogger(NheaLoggerProvider loggerProvider, string categoryName)
        {
            _provider = loggerProvider;
            _category = categoryName;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logger.LogCore(logLevel, Settings.Log.PublishType, _category, "", $"{formatter(state, exception)}", exception, Settings.Log.AutoInform);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
