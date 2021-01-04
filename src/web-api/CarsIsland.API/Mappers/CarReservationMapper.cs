using CarsIsland.API.Dto;
using CarsIsland.Core.Entities;

namespace CarsIsland.API.Mappers
{
    public static class CarReservationMapper
    {
        public static CarReservationDto MapToDto(CarReservation carReservation)
        {
            return new CarReservationDto
            {
                CarId = carReservation.CarId,
                RentFrom = carReservation.RentFrom,
                RentTo = carReservation.RentTo
            };
        }

        public static CarReservation MapFromDto(CarReservationDto carReservationDto)
        {
            return new CarReservation
            {
                CarId = carReservationDto.CarId,
                RentFrom = carReservationDto.RentFrom,
                RentTo = carReservationDto.RentTo
            };
        }
    }
}
