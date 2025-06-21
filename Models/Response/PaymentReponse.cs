namespace TiendanaMP.SDK.Models
{
    /// <summary>
    /// Representa la respuesta de un pago procesado por Mercado Pago.
    /// Contiene los datos más relevantes del estado y detalles de la transacción.
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// Identificador único del pago generado por Mercado Pago.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Estado actual del pago (por ejemplo: approved, pending, rejected).
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Detalle más específico del estado (por ejemplo: accredited, cc_rejected_insufficient_amount).
        /// </summary>
        public string? StatusDetail { get; set; }

        /// <summary>
        /// Monto total de la transacción.
        /// </summary>
        public decimal? TransactionAmount { get; set; }

        /// <summary>
        /// Descripción del producto o servicio pagado.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Identificador del método de pago utilizado (por ejemplo: visa, pse).
        /// </summary>
        public string? PaymentMethodId { get; set; }

        /// <summary>
        /// Tipo de pago (por ejemplo: credit_card, debit_card, account_money, pse).
        /// </summary>
        public string? PaymentTypeId { get; set; }

        /// <summary>
        /// Fecha de creación del pago en el sistema de Mercado Pago.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Fecha en la que el pago fue aprobado (si aplica).
        /// </summary>
        public DateTime? DateApproved { get; set; }
    }
}
