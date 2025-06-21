using System.Text.Json.Serialization;

namespace MercadoPagoAPI.SDK.Models;

/// <summary>
/// Representa una solicitud para refrescar el token de acceso OAuth en Mercado Pago.
/// </summary>
public class RefreshRequest
{
    /// <summary>
    /// Token de refresco obtenido previamente durante el flujo de autenticación OAuth (obligatorio).
    /// Se utiliza para solicitar un nuevo access_token sin requerir una nueva autorización del usuario.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }

    /// <summary>
    /// Propiedad opcional de ejemplo que solo se incluirá en el JSON si no es null.
    /// Puede ser utilizada para enviar datos adicionales opcionales.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SomeOptionalProperty { get; set; }
}
