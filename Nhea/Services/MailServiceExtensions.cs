using Nhea.Communication;
using Nhea.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MailServiceExtensions
    {
        public static IServiceCollection AddMailService(this IServiceCollection services, Action<NheaCommunicationConfigurationSettings> configureOptions)
        {
            services.Configure(configureOptions);

            services.AddSingleton<IMailService, MailService>();

            return services;
        }
    }
}
