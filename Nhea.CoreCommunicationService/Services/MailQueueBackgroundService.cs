using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nhea.Communication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nhea.CoreCommunicationService.Services
{
    public class MailQueueBackgroundService : BackgroundService
    {
        public MailQueueBackgroundService(IMailService mailService, ILogger<MailQueueBackgroundService> logger)
        {
            MailService = mailService;
            Logger = logger;
        }

        public IMailService MailService { get; }
        public ILogger<MailQueueBackgroundService> Logger { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        var mailQueueService = new Web.Services.ScheduledServices.MailQueueService(Logger);
                        mailQueueService.StartService(stoppingToken);

                    }, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}
