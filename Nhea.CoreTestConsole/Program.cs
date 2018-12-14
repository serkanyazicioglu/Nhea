using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Nhea.CoreTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(configure =>
                configure.AddConsole()
            );

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Nhea.Logging.Logger.LogPublishing += Logger_LogPublishing;
            Nhea.Logging.Logger.LogPublished += Logger_LogPublished;

            //Static logging
            Nhea.Logging.Logger.Log("Hello World!");

            //Microsoft.Extensions.Logging.ILogger logging for DI


            //Log.Information("Hello, world!");

            //logger.Log(LogLevel.Critical, default(EventId), 

            Nhea.Communication.MailQueue.MailQueueing += MailQueue_MailQueueing;
            Nhea.Communication.MailQueue.MailQueued += MailQueue_MailQueued;

            var attachmentData = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Assets/sample-image.jpg"));
            Nhea.Communication.MailQueue.Add("garajinformation@gmail.com", "serkanyazicioglu@gmail.com", "subject", "body", new Communication.MailQueueAttachment { Name = "sample-image.jpg", Data = attachmentData });

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
