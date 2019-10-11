using Microsoft.Extensions.Logging;
using Nhea.Configuration;
using Nhea.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LogServiceExtensions
    {
        public static ILoggingBuilder AddNheaLogger(this ILoggingBuilder builder, Action<NheaLogConfigurationSettings> configure)
        {
            builder.Services.Configure(configure);

            builder.Services.AddSingleton<ILoggerProvider, NheaLoggerProvider>();

            return builder;
        }
    }
}
