using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models
{
    public class FinancialInstitution
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
