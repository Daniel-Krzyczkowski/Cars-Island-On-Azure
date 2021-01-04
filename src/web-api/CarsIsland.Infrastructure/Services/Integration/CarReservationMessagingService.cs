using Azure.Messaging.ServiceBus;
using CarsIsland.Core.Entities;
using CarsIsland.Infrastructure.Messages;
using CarsIsland.Infrastructure.Services.Integration.Interfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarsIsland.Infrastructure.Services.Integration
{
    public class CarReservationMessagingService : ICarReservationMessagingService
    {
        private readonly ServiceBusSender _serviceBusSender;

        public CarReservationMessagingService(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender
                                ?? throw new ArgumentNullException(nameof(serviceBusSender));
        }

        public async Task PublishNewCarReservationMessageAsync(CarReservation carReservation)
        {
            var carReservationIntegrationMessage = new CarReservationIntegrationMessage
            {
                Id = Guid.NewGuid().ToString(),
                CarId = carReservation.CarId,
                CustomerId = carReservation.CustomerId,
                RentFrom = carReservation.RentFrom,
                RentTo = carReservation.RentTo
            };

            var serializedMessage = JsonSerializer.Serialize(carReservationIntegrationMessage);
            ServiceBusMessage message = new ServiceBusMessage(serializedMessage);
            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
