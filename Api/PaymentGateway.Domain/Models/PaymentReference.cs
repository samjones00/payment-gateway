namespace PaymentGateway.Domain.Models
{
    public class PaymentReference
    {
        public PaymentReference(string paymentReference)
        {
            Value = paymentReference;
        }

        public string Value { get; init; }
    }
}