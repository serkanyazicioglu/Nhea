using Nhea.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nhea.FrameworkCommunicationService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var mailQueueService = new Nhea.Web.Services.ScheduledServices.MailQueueService();
                    mailQueueService.StartService();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            });

            while (true)
            {
                Thread.Sleep(10000);
            }
        }
    }
}
