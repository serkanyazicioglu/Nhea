using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nhea.Configuration.GenericConfigSection.Communication;
using Nhea.CoreCommunicationService.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace Nhea.CoreCommunicationService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlConnectionString = Environment.GetEnvironmentVariable("MAILQUEUE_CONNECTION");

            IEnumerable<SmtpElement> smtpSettings = null;
            string dataFormat = Environment.GetEnvironmentVariable("MAILQUEUE_DATAFORMAT");

            string smtpSettingsStr = Environment.GetEnvironmentVariable("MAILQUEUE_SMTPSETTINGS");

            if (string.IsNullOrEmpty(smtpSettingsStr))
            {
                string smtpSettingsFilePath = Environment.GetEnvironmentVariable("MAILQUEUE_SMTPSETTINGS_FILE");

                if (!string.IsNullOrEmpty(smtpSettingsFilePath))
                {
                    smtpSettingsStr = File.ReadAllText(smtpSettingsFilePath);
                }
            }

            if (!string.IsNullOrEmpty(smtpSettingsStr))
            {
                if (dataFormat == "xml")
                {
                    var serializer = new XmlSerializer(typeof(IEnumerable<SmtpElement>));
                    using (MemoryStream stream = new(System.Text.Encoding.UTF8.GetBytes(smtpSettingsStr)))
                    {
                        smtpSettings = (IEnumerable<SmtpElement>)serializer.Deserialize(stream);
                    }
                }
                else
                {
                    smtpSettings = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<SmtpElement>>(smtpSettingsStr);
                }
            }

            if (Environment.GetEnvironmentVariable("MAILQUEUE_IGNORE_SSL_VALIDATION") == "true")
            {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
#pragma warning restore SYSLIB0014 // Type or member is obsolete
            }

            services.AddMailService(configure =>
            {
                configure.ConnectionString = sqlConnectionString;
                configure.SmtpSettings = smtpSettings;
            });

            services.AddLogging(configure => configure.AddConsole());

            services.AddLogging(configure => configure.AddNheaLogger(nhea =>
            {
                nhea.PublishType = Nhea.Logging.PublishTypes.File;
                nhea.AutoInform = false;
                nhea.FriendlyName = "nhea mail service";
            }));

            services.AddOptions();

            services.AddHostedService<MailQueueBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("It works!");
                });
            });
        }
    }
}
