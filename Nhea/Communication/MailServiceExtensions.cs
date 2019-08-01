using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nhea.Communication
{
    public static class MailServiceExtensions
    {
        public static IServiceCollection AddMailService(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IMailService, MailService>();

            // Add configuration action for WhatsAppSettings
            //services.Configure<WhatsAppSettings>(options =>
            //{
            //    if (options.ApiUrl == Constants.DefaultUrl)
            //    {
            //        // if we're using the hosted service URL, use the correct region
            //        options.Region = Constants.DefaultHostedRegion;
            //    }
            //});
            return services;
        }
    }
}
