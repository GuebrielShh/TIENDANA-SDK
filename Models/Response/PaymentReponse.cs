namespace TiendanaMP.SDK.Models;
public class PaymentResponse
{
    public long Id { get; set; }
    public string? Status { get; set; }
    public string? StatusDetail { get; set; }
    public decimal? TransactionAmount { get; set; }
    public string? Description { get; set; }
    public string? PaymentMethodId { get; set; }
    public string? PaymentTypeId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateApproved { get; set; }

}
