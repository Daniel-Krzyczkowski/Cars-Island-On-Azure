using System.Threading.Tasks;

namespace CarsIsland.MailSender.Infrastructure.Services.Integration.Interfaces
{
    public interface ICarReservationMessagingService
    {
        Task HandleNewCarReservationMessageAsync(string carReservationIntegrationMessageAsString);
    }
}

