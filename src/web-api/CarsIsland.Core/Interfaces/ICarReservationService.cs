using CarsIsland.Core.Common;
using CarsIsland.Core.Entities;
using System.Threading.Tasks;

namespace CarsIsland.Core.Interfaces
{
    public interface ICarReservationService
    {
        Task<OperationResponse<CarReservation>> MakeReservationAsync(CarReservation carReservation);
    }
}
