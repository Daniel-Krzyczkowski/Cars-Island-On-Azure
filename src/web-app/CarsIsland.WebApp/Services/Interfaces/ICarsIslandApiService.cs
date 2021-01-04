using CarsIsland.WebApp.Common;
using CarsIsland.WebApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsIsland.WebApp.Services.Interfaces
{
    public interface ICarsIslandApiService
    {
        Task<IReadOnlyCollection<Car>> GetAvailableCarsAsync();
        Task SendEnquiryAsync(string attachmentFileName, CustomerEnquiry enquiry);
        Task<OperationResponse> CreateNewCarReservationAsync(CarReservation carReservation);
    }
}
