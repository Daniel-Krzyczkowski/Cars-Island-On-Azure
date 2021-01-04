using CarsIsland.Core.Entities;
using System.Threading.Tasks;

namespace CarsIsland.Infrastructure.Services.Integration.Interfaces
{
    public interface ICarReservationMessagingService
    {
        Task PublishNewCarReservationMessageAsync(CarReservation carReservation);
    }
}
