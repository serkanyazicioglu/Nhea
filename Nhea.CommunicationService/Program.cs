using System;
using System.Threading.Tasks;

namespace Nhea.CommunicationService
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => {
                Nhea.Web.Services.ScheduledServices.MailQueueService mailQueueService = new Web.Services.ScheduledServices.MailQueueService();
                mailQueueService.StartService();
            });

            Console.ReadLine();
        }
    }
}
