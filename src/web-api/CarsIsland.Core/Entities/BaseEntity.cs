using System.Text.Json.Serialization;

namespace CarsIsland.Core.Entities
{
    public abstract class BaseEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
