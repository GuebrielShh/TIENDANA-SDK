using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models
{

    
    public class PaymentMethod
    {
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("financial_institutions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<FinancialInstitution>? FinancialInstitutions { get; set; }
    }
}


