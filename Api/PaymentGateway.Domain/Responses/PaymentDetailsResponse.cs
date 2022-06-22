namespace PaymentGateway.Domain.Responses
{
    public class PaymentDetailsResponse
    {
        public DateTime ProcessedOn { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsAuthorised { get; set; }
        public string CardNumber { get; set; } = string.Empty;
    }
}
