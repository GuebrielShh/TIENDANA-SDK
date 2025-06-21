
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models.Response
{
    /// <summary>
    /// Representa la respuesta del servidor de autenticación de Mercado Pago al solicitar un token de acceso.
    /// Este modelo encapsula tanto el access_token como información relacionada al usuario y expiración.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Token de acceso válido para hacer llamadas autenticadas a la API de Mercado Pago.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string? Access_token { get; set; }

        /// <summary>
        /// Tipo de token (normalmente "bearer").
        /// </summary>
        [JsonPropertyName("token_type")]
        public string? Token_type { get; set; }

        /// <summary>
        /// Tiempo de expiración del token en segundos.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int Expires_in { get; set; }

        /// <summary>
        /// Alcance de los permisos concedidos por el token.
        /// </summary>
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        /// <summary>
        /// Token de refresco para obtener nuevos tokens sin necesidad de reautenticación.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string? Refresh_token { get; set; }

        /// <summary>
        /// ID del usuario (vendedor) autenticado en la plataforma de Mercado Pago.
        /// </summary>
        [JsonPropertyName("user_id")]
        public long? User_id { get; set; }

        /// <summary>
        /// Fecha y hora (UTC) en que se obtuvo el token.
        /// Esta propiedad no la devuelve la API, pero debe establecerse manualmente al guardar el token.
        /// </summary>
        public DateTime ObtainedAt { get; set; } = DateTime.UtcNow;
    }
}
