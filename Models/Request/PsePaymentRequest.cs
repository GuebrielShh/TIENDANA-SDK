using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TiendanaMP.SDK.Models.Token_Request

{
    public class PsePaymentRequest
    {
        [Required]
        [JsonPropertyName("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [Required]
        [JsonPropertyName("payment_method_id")]
        public string PaymentMethodId { get; set; } = "pse";

        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("payer")]
        public PsePayer Payer { get; set; } = new();

        [Required]
        [JsonPropertyName("callback_url")]
        [Url]
        public string CallbackUrl { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("notification_url")]
        [Url]
        public string NotificationUrl { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("transaction_details")]
        public PseTransactionDetails TransactionDetails { get; set; } = new();
    }

    public class PsePayer
    {
        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("identification")]
        public PseIdentification Identification { get; set; } = new();

        [Required]
        [JsonPropertyName("entity_type")]
        public string EntityType { get; set; } = "individual"; // o "association"

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = "customer";

        [Required]
        [JsonPropertyName("additional_info")]
        public PseAdditionalInfo AdditionalInfo { get; set; } = new();
    }

    public class PseIdentification
    {
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = "CC";

        [Required]
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;
    }

    public class PseAdditionalInfo
    {
        [Required]
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("financial_institution")]
        public string FinancialInstitution { get; set; } = string.Empty;
    }

    public class PseTransactionDetails
    {
        [Required]
        [JsonPropertyName("financial_institution")]
        public string FinancialInstitution { get; set; } = string.Empty;
    }
}
