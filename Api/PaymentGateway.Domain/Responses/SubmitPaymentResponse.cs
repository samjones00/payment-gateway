namespace PaymentGateway.Domain.Responses
{
    public class SubmitPaymentResponse
    {
        public string PaymentStatus { get; set; }
        public string PaymentReference { get; set; }
    }
}