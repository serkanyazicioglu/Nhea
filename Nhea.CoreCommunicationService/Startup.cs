using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nhea.Configuration.GenericConfigSection.Communication;
using Nhea.CoreCommunicationService.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
                    XmlSerializer serializer = new XmlSerializer(typeof(IEnumerable<SmtpElement>));
                    using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(smtpSettingsStr)))
                    {
                        smtpSettings = (IEnumerable<SmtpElement>)serializer.Deserialize(stream);
                    }
                }
                else
                {
                    smtpSettings = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<SmtpElement>>(smtpSettingsStr);
                }
            }

            services.AddMailService(configure =>
            {
                configure.ConnectionString = sqlConnectionString;
                configure.SmtpSettings = smtpSettings;
            });

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
