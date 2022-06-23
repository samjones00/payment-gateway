namespace PaymentGateway.AcquiringBank.MockCKOBank.Models
{
    public class MockResponse
    {
        public string PaymentReference { get; init; }
        public string Status { get; init; }

        public MockResponse(string paymentReference, string status)
        {
            PaymentReference = paymentReference;
            Status = status;
        }
    }
}
