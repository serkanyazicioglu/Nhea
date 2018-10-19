using System;

namespace Nhea.CoreTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Nhea.Logging.Logger.LogPublishing += Logger_LogPublishing;
            Nhea.Logging.Logger.LogPublished += Logger_LogPublished;

            //Nhea.Communication.MailQueue.MailQueueing += MailQueue_MailQueueing;
            //Nhea.Communication.MailQueue.MailQueued += MailQueue_MailQueued;

            try
            {
                throw new Exception("Test Exception");
            }
            catch (Exception ex)
            {
                ex.Data.Add("Id", 1);
                Nhea.Logging.Logger.Log(ex);
            }

            Console.ReadLine();
        }

        private static void MailQueue_MailQueueing(Communication.Mail mail)
        {
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
