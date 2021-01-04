using CarsIsland.MailSender.Infrastructure.Services.Integration.Interfaces;
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;

namespace CarsIsland.MailSender.FuncApp
{
    public class MailSenderFuncApp
    {
        private readonly ICarReservationMessagingService _carReservationMessagingService;

        public MailSenderFuncApp(ICarReservationMessagingService carReservationMessagingService)
        {
            _carReservationMessagingService = carReservationMessagingService
                                                    ?? throw new ArgumentNullException(nameof(carReservationMessagingService));
        }

        [FunctionName("mail-sender-func-app")]
        public async Task RunAsync([ServiceBusTrigger("reservations", Connection = "AzureServiceBusConnectionString")] string queueItem)
        {
            await _carReservationMessagingService.HandleNewCarReservationMessageAsync(queueItem);
        }
    }
}
