using CarsIsland.Core.Entities;
using System;
using System.Threading.Tasks;

namespace CarsIsland.Core.Interfaces
{
    public interface ICarReservationRepository : IDataRepository<CarReservation>
    {
        Task<CarReservation> GetExistingReservationByCarIdAsync(string carId, DateTime rentFrom);
    }
}
