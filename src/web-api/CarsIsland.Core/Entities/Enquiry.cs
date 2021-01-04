using System.Text.Json.Serialization;

namespace CarsIsland.Core.Entities
{
    public class Enquiry : BaseEntity
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("customerContactEmail")]
        public string CustomerContactEmail { get; set; }
        [JsonPropertyName("attachmentUrl")]
        public string AttachmentUrl { get; set; }
    }
}
