using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models.Token_Request
{
    /// <summary>
    /// Representa una solicitud de pago con tarjeta en Mercado Pago.
    /// Contiene los datos mínimos necesarios para generar un pago con un token de tarjeta.
    /// </summary>
    public class CardPaymentRequest
    {
        /// <summary>
        /// Monto total de la transacción (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        /// <summary>
        /// Token de la tarjeta generada desde el frontend o cliente (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del producto o motivo del pago (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Número de cuotas para el pago (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("installments")]
        public int Installments { get; set; }

        /// <summary>
        /// ID del método de pago (por ejemplo: visa, mastercard) (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("payment_method_id")]
        public string PaymentMethodId { get; set; } = string.Empty;

        /// <summary>
        /// ID del emisor de la tarjeta, obtenido desde la API de Mercado Pago (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("issuer_id")]
        public string IssuerId { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del pagador (obligatorio y debe ser un email válido).
        /// </summary>
        [Required]
        [EmailAddress]
        [JsonPropertyName("payer_email")]
        public string PayerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del titular de la tarjeta (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("cardholder_name")]
        public string CardholderName { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de documento del titular de la tarjeta (por defecto "CC") (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("cardholder_identification_type")]
        public string CardholderIdentificationType { get; set; } = "CC";

        /// <summary>
        /// Número de documento del titular de la tarjeta (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("cardholder_identification_number")]
        public string CardholderIdentificationNumber { get; set; } = string.Empty;

        /// <summary>
        /// URL para recibir notificaciones del estado del pago (obligatorio y debe ser una URL válida).
        /// </summary>
        [Required]
        [Url]
        [JsonPropertyName("notification_url")]
        public string NotificationUrl { get; set; } = string.Empty;

        /// <summary>
        /// Dirección IP del cliente que realiza el pago (opcional).
        /// </summary>
        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }
    }

    /// <summary>
    /// Representa información del titular de la tarjeta.
    /// Este modelo puede ser usado para encapsular los datos del cardholder por separado si se requiere.
    /// </summary>
    public class CardholderInfo
    {
        /// <summary>
        /// Nombre completo del titular de la tarjeta (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("cardholder_name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de identificación del titular (por defecto "CC") (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("cardholder_identification_type")]
        public string IdentificationType { get; set; } = "CC";

        /// <summary>
        /// Número de identificación del titular de la tarjeta (obligatorio).
        /// </summary>
        [Required]
        [JsonPropertyName("cardholder_identification_number")]
        public string IdentificationNumber { get; set; } = string.Empty;
    }
}
