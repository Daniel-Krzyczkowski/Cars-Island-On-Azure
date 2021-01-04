using System;
using System.ComponentModel.DataAnnotations;

namespace CarsIsland.API.Dto
{
    public class CarReservationDto
    {
        [Required]
        public string CarId { get; set; }
        [Required]
        public DateTime RentFrom { get; set; }
        [Required]
        public DateTime RentTo { get; set; }
    }
}
