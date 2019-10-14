using System;
using System.IO;

namespace Nhea.FrameworkTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Nhea.Logging.Logger.LogPublishing += Logger_LogPublishing;
            Nhea.Logging.Logger.LogPublished += Logger_LogPublished;

            Nhea.Communication.MailQueue.MailQueueing += MailQueue_MailQueueing;
            Nhea.Communication.MailQueue.MailQueued += MailQueue_MailQueued;

            Nhea.Logging.Logger.Log("Hello World!");

            var testException = new Exception("Test exception");
            testException.Data.Add("Now", DateTime.Now.ToString());

            Nhea.Logging.Logger.Log(testException);

            var attachmentData = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Assets/sample-image.jpg"));

            string from = "from@domain.com";
            string to = "to@domain.com";
            string cc = null;
            string bcc = null;
            string subject = "subject";
            string body = "<html><body>html body text</body></html>";

            Nhea.Communication.MailQueue.Add(from, to, subject, body, new Communication.MailQueueAttachment
            {
                Name = "sample-image.jpg",
                Data = attachmentData
            });

            Nhea.Communication.MailQueue.Add(from, to, subject, body, new Communication.MailQueueAttachment
            {
                Name = "sample-image.jpg",
                Data = attachmentData
            }, listUnsubscribe: "http://domain.com/unsubscribe", plainText: "plainbody");

            Nhea.Communication.MailQueue.Add(from, to, cc, subject, body);

            Nhea.Communication.MailQueue.Add(from, to, cc, bcc, subject, body);

            Nhea.Communication.MailQueue.Add(from, to, cc, bcc, subject, body, "https://www.testdomain.com/unsub");

            Nhea.Communication.MailQueue.Add(from, to, cc, bcc, subject, body, "https://www.testdomain.com/unsub", "plainbody");

            Console.WriteLine("Job Done!");
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
