using System;
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models.Response
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? Access_token { get; set; }

        [JsonPropertyName("token_type")]
        public string? Token_type { get; set; }

        [JsonPropertyName("expires_in")]
        public int Expires_in { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? Refresh_token { get; set; }

        [JsonPropertyName("user_id")]
        public long? User_id { get; set; }

        // ✅ Esta propiedad no la devuelve Mercado Pago, pero debes establecerla manualmente cuando guardes el token
        public DateTime ObtainedAt { get; set; } = DateTime.UtcNow;
    }
}
