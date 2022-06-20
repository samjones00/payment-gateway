namespace PaymentGateway.Domain.Models
{
    public class PaymentReference
    {
        public const int Length = 36;
        public string Value { get; private set; }

        public static PaymentReference Create(string paymentReference)
        {
            return new PaymentReference
            {
                Value = paymentReference
            };
        }
    }
}