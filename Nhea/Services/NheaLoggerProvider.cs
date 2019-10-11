using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nhea.Configuration;
using System.Collections.Concurrent;

namespace Nhea.Services
{
    public class NheaLoggerProvider : ILoggerProvider
    {
        public NheaLoggerProvider(IOptions<NheaLogConfigurationSettings> nheaConfigurationSettings)
        {
            Settings.CurrentLogConfigurationSettings = nheaConfigurationSettings.Value;
        }

        public ConcurrentDictionary<string, NheaLogger> Loggers { get; set; } = new ConcurrentDictionary<string, NheaLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return Loggers.GetOrAdd(categoryName, new NheaLogger(this, categoryName));
        }

        public void Dispose()
        {
        }
    }
}
