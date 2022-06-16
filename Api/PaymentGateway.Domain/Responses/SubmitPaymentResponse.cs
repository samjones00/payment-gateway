using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Core.Responses
{
    public class SubmitPaymentResponse
    {
        public PaymentStatus PaymentStatus { get; set; }
        public IEnumerable<string> ValidationErrors { get; set; }
        public bool IsSuccess { get; set; }
        public string PaymentReference { get; set; }
    }
}
