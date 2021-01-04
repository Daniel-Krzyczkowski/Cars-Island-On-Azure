using System;
using System.Text.Json.Serialization;

namespace CarsIsland.Core.Entities
{
    public class CarReservation : BaseEntity
    {
        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; }
        [JsonPropertyName("carId")]
        public string CarId { get; set; }
        [JsonPropertyName("rentFrom")]
        public DateTime RentFrom { get; set; }
        [JsonPropertyName("rentTo")]
        public DateTime RentTo { get; set; }
    }
}
