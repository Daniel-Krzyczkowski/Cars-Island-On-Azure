using CarsIsland.MailSender.Core.Entities;
using CarsIsland.MailSender.Core.Interfaces;
using CarsIsland.MailSender.Infrastructure.Messages;
using CarsIsland.MailSender.Infrastructure.Services.Integration.Interfaces;
using CarsIsland.MailSender.Infrastructure.Services.Mail.Interfaces;
using CarsIsland.MailSender.Infrastructure.Services.Mail.Templates;
using CarsIsland.MailSender.Infrastructure.Services.MsGraph.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarsIsland.MailSender.Infrastructure.Services.Integration
{
    public class CarReservationMessagingService : ICarReservationMessagingService
    {
        private readonly IMsGraphSdkClientService _msGraphSdkClientService;
        private readonly IMailDeliveryService _mailDeliveryService;
        private readonly IDataRepository<Car> _carRepository;
        private readonly ILogger<CarReservationMessagingService> _logger;

        public CarReservationMessagingService(IMsGraphSdkClientService msGraphSdkClientService,
                                              IMailDeliveryService mailDeliveryService,
                                              IDataRepository<Car> carRepository,
                                              ILogger<CarReservationMessagingService> logger)
        {
            _msGraphSdkClientService = msGraphSdkClientService
                                       ?? throw new ArgumentNullException(nameof(msGraphSdkClientService));

            _mailDeliveryService = mailDeliveryService
                                       ?? throw new ArgumentNullException(nameof(mailDeliveryService));

            _carRepository = carRepository
                                       ?? throw new ArgumentNullException(nameof(carRepository));

            _logger = logger
                                       ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleNewCarReservationMessageAsync(string carReservationIntegrationMessageAsString)
        {
            var carReservationIntegrationMessage = JsonSerializer.Deserialize<CarReservationIntegrationMessage>(carReservationIntegrationMessageAsString);
            if (!string.IsNullOrEmpty(carReservationIntegrationMessage.CustomerId))
            {
                var customer = await _msGraphSdkClientService.GetUserAsync(carReservationIntegrationMessage.CustomerId);

                if (customer != null)
                {

                    var carFromReservation = await _carRepository.GetAsync(carReservationIntegrationMessage.CarId);

                    if (carFromReservation != null)
                    {
                        var carReservationConfirmationMailTemplate = new CarReservationConfirmationMailTemplate
                        {
                            CustomerName = $"{customer.FirstName} {customer.LastName}",
                            CustomerEmail = customer.Email,
                            CarBrand = carFromReservation.Brand,
                            CarModel = carFromReservation.Model,
                            CarImageUrl = carFromReservation.ImageUrl,
                            FromDate = carReservationIntegrationMessage.RentFrom.ToString("dd/MM/yyyy"),
                            ToDate = carReservationIntegrationMessage.RentTo.ToString("dd/MM/yyyy")
                        };

                        await _mailDeliveryService.SendInvitationMessageAsync(carReservationConfirmationMailTemplate);
                    }
                }
            }

            else
            {
                _logger.LogError($"Customer ID value in the queue message cannot be empty. Row content: {carReservationIntegrationMessageAsString}");
            }
        }
    }
}
