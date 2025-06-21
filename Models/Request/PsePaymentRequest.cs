using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models.Token_Request
{
    /// <summary>
    /// Representa la solicitud de pago PSE para Mercado Pago.
    /// Contiene toda la información necesaria para generar un pago vía débito desde cuenta bancaria.
    /// </summary>
    public class PsePaymentRequest
    {
        /// <summary>
        /// Monto de la transacción (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// ID del método de pago (debe ser "pse") (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("payment_method_id")]
        public string PaymentMethodId { get; set; } = "pse";

        /// <summary>
        /// Descripción del pago o producto (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Información del pagador (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("payer")]
        public PsePayer Payer { get; set; } = new();

        /// <summary>
        /// URL a la que será redirigido el cliente después del intento de pago (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("callback_url")]
        [Url]
        public string CallbackUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL para recibir notificaciones del estado del pago (webhook) (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("notification_url")]
        [Url]
        public string NotificationUrl { get; set; } = string.Empty;

        /// <summary>
        /// Detalles adicionales de la transacción, como la institución financiera (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("transaction_details")]
        public PseTransactionDetails TransactionDetails { get; set; } = new();
    }

    /// <summary>
    /// Contiene la información detallada del pagador en una transacción PSE.
    /// </summary>
    public class PsePayer
    {
        /// <summary>
        /// Correo electrónico del pagador (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Primer nombre del pagador (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del pagador (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Documento de identificación del pagador (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("identification")]
        public PseIdentification Identification { get; set; } = new();

        /// <summary>
        /// Tipo de entidad: puede ser "individual" o "association" (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("entity_type")]
        public string EntityType { get; set; } = "individual";

        /// <summary>
        /// Tipo de usuario. Normalmente "customer" (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = "customer";

        /// <summary>
        /// Información adicional requerida por la pasarela PSE (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("additional_info")]
        public PseAdditionalInfo AdditionalInfo { get; set; } = new();
    }

    /// <summary>
    /// Documento de identificación del pagador.
    /// </summary>
    public class PseIdentification
    {
        /// <summary>
        /// Tipo de documento (por ejemplo, "CC" para cédula de ciudadanía) (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = "CC";

        /// <summary>
        /// Número de identificación (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;
    }

    /// <summary>
    /// Información adicional necesaria para pagos PSE.
    /// </summary>
    public class PseAdditionalInfo
    {
        /// <summary>
        /// Dirección IP del cliente (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// Código o nombre de la institución financiera seleccionada (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("financial_institution")]
        public string FinancialInstitution { get; set; } = string.Empty;
    }

    /// <summary>
    /// Detalles específicos de la transacción PSE como la institución financiera.
    /// </summary>
    public class PseTransactionDetails
    {
        /// <summary>
        /// Código o identificador de la institución financiera (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("financial_institution")]
        public string FinancialInstitution { get; set; } = string.Empty;
    }
}
