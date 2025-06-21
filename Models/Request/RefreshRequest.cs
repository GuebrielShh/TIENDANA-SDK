using System.Text.Json.Serialization;

namespace MercadoPagoAPI.SDK.Models;

public class RefreshRequest
{
   [JsonPropertyName("refresh_token")]
public required string RefreshToken { get; set; }

[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
public string? SomeOptionalProperty { get; set; }

}
