namespace PaymentGateway.Domain.Responses
{
    public class PaymentDetailsResponse
    {
        public DateTime ProcessedOn { get; set; }
        public string PaymentReference { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public bool IsAuthorised { get; set; }
    }
}
