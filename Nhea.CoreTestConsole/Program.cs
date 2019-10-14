using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Nhea.Communication;
using System;
using System.IO;
using Nhea.Localization;

namespace Nhea.CoreTestConsole
{
    class Program
    {

        /// <summary>
        /// Mail kaydetme, mail queue, loglama ve localization servisi var. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddLogging(configure =>
                configure.AddConsole()
                .AddNheaLogger(nheaConfigure =>
                {
                    nheaConfigure.AutoInform = true;
                    nheaConfigure.ConnectionString = configuration.GetConnectionString("SqlConnectionString");
                    nheaConfigure.PublishType = Nhea.Logging.PublishTypes.Database;
                    nheaConfigure.MailFrom = "from@domain.com";
                    nheaConfigure.MailList = "to@domain.com;to2@domain.com";
                    nheaConfigure.InformSubject = "test subject";
                })
            );

            services.AddNheaLocalizationService(configure =>
            {
                configure.ConnectionString = configuration.GetConnectionString("SqlConnectionString");
                configure.DefaultLanguageId = 1;
            });

            services.AddMailService(configure =>
            {
                configure.ConnectionString = configuration.GetConnectionString("SqlConnectionString");
            });

            services.AddOptions();

            var serviceProvider = services.BuildServiceProvider();

            Nhea.Logging.Logger.LogPublishing += Logger_LogPublishing;
            Nhea.Logging.Logger.LogPublished += Logger_LogPublished;

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.Log(LogLevel.Information, "test core logger");

            logger.LogError(new Exception("Test exception"), "Test exception description");

            Nhea.Communication.MailQueue.MailQueueing += MailQueue_MailQueueing;
            Nhea.Communication.MailQueue.MailQueued += MailQueue_MailQueued;

            var attachmentData = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Assets/sample-image.jpg"));

            var mailService = serviceProvider.GetService<IMailService>();
            mailService.Add("from@domain.com", "to@domain.com", "subject", "body", new Communication.MailQueueAttachment
            {
                Name = "sample-image.jpg",
                Data = attachmentData
            });

            var localizationService = serviceProvider.GetService<ILocalizationService>();

            localizationService.SaveLocalization("new translation", "NewTranslation");

            var translation = localizationService.GetLocalization("NewTranslation");
            Console.WriteLine("Tanslation: " + translation);

            localizationService.DeleteLocalization("NewTranslation");

            Console.WriteLine("Job done!");
            Console.ReadLine();
        }

        private static void MailQueue_MailQueueing(Communication.Mail mail)
        {
            mail.Subject += " I'm manipulating subject!";
            Console.WriteLine("Mail Queueing: " + mail.Subject);
        }

        private static void MailQueue_MailQueued(Communication.Mail mail, bool result)
        {
            Console.WriteLine("Mail Queued: " + mail.Subject);
        }

        private static void Logger_LogPublishing(Logging.LogPublisher.Publisher publisher)
        {
            publisher.Message += " I'm manipulating log message!";

            Console.WriteLine(publisher.Message);
        }

        private static void Logger_LogPublished(Logging.LogPublisher.Publisher publisher, bool result)
        {
            Console.WriteLine("Log published! Publish result: " + result.ToString());
        }
    }
}
