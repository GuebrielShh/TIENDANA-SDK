using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models
{
    /// <summary>
    /// Representa un método de pago disponible en Mercado Pago.
    /// Puede incluir información como el nombre del método y las instituciones financieras asociadas (por ejemplo, para PSE).
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Identificador del método de pago (por ejemplo: "pse", "visa", "master").
        /// Esta propiedad se omite en el JSON si su valor es null.
        /// </summary>
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        /// <summary>
        /// Nombre legible del método de pago (por ejemplo: "Débito PSE", "Visa").
        /// Esta propiedad se omite en el JSON si su valor es null.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>
        /// Lista de instituciones financieras disponibles para este método de pago (usado principalmente con PSE).
        /// Esta propiedad se omite en el JSON si su valor es null.
        /// </summary>
        [JsonPropertyName("financial_institutions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<FinancialInstitution>? FinancialInstitutions { get; set; }
    }
}
