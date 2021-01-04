using CarsIsland.API.Dto;
using FluentValidation;
using System;

namespace CarsIsland.API.DtoValidation
{
    public class CustomerCarReservationValidator : AbstractValidator<CarReservationDto>
    {
        public CustomerCarReservationValidator()
        {
            RuleFor(x => x.CarId).NotEmpty()
                                 .NotEmpty()
                                 .Must(ValidateGuidValue)
                                 .WithMessage("Car id is not provided in the valid format. It should be a valid GUID value.");
            RuleFor(x => x.RentFrom).Must(date => date != default);
            RuleFor(x => x.RentTo).Must(date => date != default);
        }

        private bool ValidateGuidValue(string stringValue)
        {
            return Guid.TryParse(stringValue, out var result);
        }
    }
}
