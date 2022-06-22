namespace PaymentGateway.Domain.Responses
{
    public class SubmitPaymentResponse
    {
        public DateTime ProcessedOn { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentReference { get; set; } = string.Empty;
    }
}