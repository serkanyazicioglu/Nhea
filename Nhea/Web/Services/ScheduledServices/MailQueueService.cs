using System;
using System.Collections.Generic;
using System.Threading;
using Nhea.Communication;
using Nhea.Logging;

namespace Nhea.Web.Services.ScheduledServices
{
    public class MailQueueService
    {
        public MailQueueService()
        {
            SleepTime = 20000;
        }

        public int SleepTime { get; set; }

        public void StartService()
        {
            while (true)
            {
                try
                {
                    Execute();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex, false);
                }

                Thread.Sleep(SleepTime);
            }
        }

        public void Execute()
        {
            int standByPeriod = 1;

            foreach (KeyValuePair<int, MailProvider> item in MailProvider.Providers)
            {
                MailProvider provider = item.Value;

                if (provider.StandbyPeriod > standByPeriod)
                {
                    standByPeriod = provider.StandbyPeriod;
                }

                List<Mail> mailList = MailQueue.Fetch(provider);

                mailList.AddRange(MailQueue.Fetch());

                foreach (Mail mail in mailList)
                {
                    try
                    {
                        mail.Send();
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        mail.SetError();

                        Logger.Log(ex, false);
                    }
                }
            }

            Thread.Sleep(standByPeriod * 1000);
        }
    }
}
