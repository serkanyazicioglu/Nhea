using Nhea.Configuration;
using Nhea.Localization;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LocalizationServiceExtensions
    {
        public static IServiceCollection AddNheaLocalizationService(this IServiceCollection services, Action<NheaDataConfigurationSettings> configureOptions)
        {
            services.Configure(configureOptions);

            services.AddSingleton<ILocalizationService, LocalizationService>();

            return services;
        }
    }
}
