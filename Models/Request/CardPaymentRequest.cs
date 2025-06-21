using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models.Token_Request;

public class CardPaymentRequest
{
    [Required]
    [JsonPropertyName("transaction_amount")]
    public decimal TransactionAmount { get; set; }

    [Required]
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("installments")]
    public int Installments { get; set; }

    [Required]
    [JsonPropertyName("payment_method_id")]
    public string PaymentMethodId { get; set; } = string.Empty; // Ej: visa, master

    [Required]
    [JsonPropertyName("issuer_id")]
    public string IssuerId { get; set; } = string.Empty;

    // Payer email
    [Required]
    [EmailAddress]
    [JsonPropertyName("payer_email")]
    public string PayerEmail { get; set; } = string.Empty;

    // Cardholder (titular)
    [Required]
    [JsonPropertyName("cardholder_name")]
    public string CardholderName { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("cardholder_identification_type")]
    public string CardholderIdentificationType { get; set; } = "CC";

    [Required]
    [JsonPropertyName("cardholder_identification_number")]
    public string CardholderIdentificationNumber { get; set; } = string.Empty;

    [Required]
    [Url]
    [JsonPropertyName("notification_url")]
    public string NotificationUrl { get; set; } = string.Empty;

    // IP del cliente (puedes enviarla opcionalmente si la capturas)
    [JsonPropertyName("ip_address")]
    public string? IpAddress { get; set; }
}


public class CardholderInfo
{
    [Required]
    [JsonPropertyName("cardholder_name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("cardholder_identification_type")]
    public string IdentificationType { get; set; } = "CC";

    [Required]
    [JsonPropertyName("cardholder_identification_number")]
    public string IdentificationNumber { get; set; } = string.Empty;
}

