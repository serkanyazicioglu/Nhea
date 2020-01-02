using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nhea.CoreCommunicationService.Services
{
    public class MailQueueBackgroundService : BackgroundService
    {
        public MailQueueBackgroundService(ILogger<MailQueueBackgroundService> logger)
        {
            Logger = logger;
        }

        public ILogger<MailQueueBackgroundService> Logger { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        Nhea.Web.Services.ScheduledServices.MailQueueService mailQueueService = new Web.Services.ScheduledServices.MailQueueService();
                        mailQueueService.StartService();

                    }, stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}
