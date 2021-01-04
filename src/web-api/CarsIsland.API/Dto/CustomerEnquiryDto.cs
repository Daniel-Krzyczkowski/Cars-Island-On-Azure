using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarsIsland.API.Dto
{
    public class CustomerEnquiryDto
    {
        [Required]
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [Required]
        [JsonPropertyName("customerContactEmail")]
        public string CustomerContactEmail { get; set; }

        public IFormFile Attachment { get; set; }
    }
}
