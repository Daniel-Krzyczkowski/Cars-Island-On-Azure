using System;
using System.Text.Json.Serialization;

namespace CarsIsland.WebApp.Data
{
    public class CarReservation
    {
        [JsonPropertyName("carId")]
        public string CarId { get; set; }
        [JsonPropertyName("rentFrom")]
        public DateTime? RentFrom { get; set; }
        [JsonPropertyName("rentTo")]
        public DateTime? RentTo { get; set; }
    }
}
