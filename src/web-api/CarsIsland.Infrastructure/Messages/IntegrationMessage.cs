using System.Text.Json.Serialization;

namespace CarsIsland.Infrastructure.Messages
{
    public abstract class IntegrationMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
