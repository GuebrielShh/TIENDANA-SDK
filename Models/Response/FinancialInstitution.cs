using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models
{
    /// <summary>
    /// Representa una institución financiera habilitada para pagos PSE.
    /// Esta clase puede utilizarse para listar los bancos disponibles desde la API de Mercado Pago.
    /// </summary>
    public class FinancialInstitution
    {
        /// <summary>
        /// Identificador único de la institución financiera (por ejemplo, el código usado por PSE o Mercado Pago).
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Nombre de la institución financiera (banco) visible para el usuario.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
