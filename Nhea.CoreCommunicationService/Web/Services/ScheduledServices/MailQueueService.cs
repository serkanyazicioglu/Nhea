using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Nhea.Communication;
using Nhea.CoreCommunicationService;
using Nhea.Logging;

namespace Nhea.Web.Services.ScheduledServices
{
    public class MailQueueService
    {
        private ILogger CurrentLogger { get; set; }

        public MailQueueService(ILogger logger = null)
        {
            CurrentLogger = logger;
            SleepTime = 10000;
        }

        public MailQueueService(int sleepTime, ILogger logger = null)
        {
            CurrentLogger = logger;
            SleepTime = sleepTime;
        }

        public int SleepTime { get; set; }

        public void StartService()
        {
            while (true)
            {
                int mailsSent = 0;

                try
                {
                    mailsSent = Execute();
                }
                catch (Exception ex)
                {
                    if (CurrentLogger != null)
                    {
                        CurrentLogger.LogError(ex, ex.Message);
                    }
                    else
                    {
                        Logger.Log(ex, false);
                    }
                }

                if (mailsSent == 0)
                {
                    Thread.Sleep(SleepTime);
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }

        public int Execute()
        {
            int mailsSent = 0;

            foreach (KeyValuePair<int, MailProvider> item in MailProvider.Providers)
            {
                int standByPeriod = 1;

                MailProvider provider = item.Value;

                if (provider.StandbyPeriod > standByPeriod)
                {
                    standByPeriod = provider.StandbyPeriod;
                }

                var providerMailList = MailQueueProcessor.Fetch(provider);

                SendMailList(providerMailList);

                mailsSent += providerMailList.Count;

                Thread.Sleep(standByPeriod * 1000);
            }

            var nonProviderMailList = MailQueueProcessor.Fetch();
            SendMailList(nonProviderMailList);

            mailsSent += nonProviderMailList.Count;

            return mailsSent;
        }

        private void SendMailList(List<Nhea.CoreCommunicationService.Mail> mailList)
        {
            foreach (var mail in mailList)
            {
                try
                {
                    mail.Send();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    mail.SetError();

                    if (CurrentLogger != null)
                    {
                        CurrentLogger.LogError(ex, ex.Message);
                    }
                    else
                    {
                        Logger.Log(ex, false);
                    }
                }
            }
        }
    }
}
