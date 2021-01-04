using CarsIsland.Core.Common;
using CarsIsland.Core.Entities;
using CarsIsland.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace CarsIsland.Core.Services
{
    public class CarReservationService : ICarReservationService
    {
        private readonly ICarReservationRepository _carReservationRepository;
        private readonly IDataRepository<Car> _carRepository;
        private readonly IIdentityService _identityService;

        public CarReservationService(ICarReservationRepository carReservationRepository,
                                     IDataRepository<Car> carRepository,
                                     IIdentityService identityService)
        {

            _carReservationRepository = carReservationRepository
                                       ?? throw new ArgumentNullException(nameof(carReservationRepository));


            _carRepository = carRepository
                                       ?? throw new ArgumentNullException(nameof(carRepository));

            _identityService = identityService
                                       ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<OperationResponse<CarReservation>> MakeReservationAsync(CarReservation carReservation)
        {
            var carFromReservation = await _carRepository.GetAsync(carReservation.CarId);
            if (carFromReservation == null)
            {
                return new OperationResponse<CarReservation>()
                                       .SetAsFailureResponse(OperationErrorDictionary.CarReservation.CarDoesNotExist());
            }

            var existingCarReservation = await _carReservationRepository.GetExistingReservationByCarIdAsync(carReservation.CarId, carReservation.RentFrom);

            if (existingCarReservation != null)
            {
                return new OperationResponse<CarReservation>()
                                       .SetAsFailureResponse(OperationErrorDictionary.CarReservation.CarAlreadyReserved());
            }

            else
            {
                carReservation.Id = Guid.NewGuid().ToString();
                carReservation.CustomerId = _identityService.GetUserIdentity().ToString();
                var createdCarReservation = await _carReservationRepository.AddAsync(carReservation);
                return new OperationResponse<CarReservation>(createdCarReservation);
            }
        }
    }
}
