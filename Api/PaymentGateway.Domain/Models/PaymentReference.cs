using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models
{
    public class PaymentReference
    {
        public const int Length = 36;
        public string Value { get; private set; }

        public static PaymentReference Create(string paymentReference)
        {
            paymentReference.ThrowIfNullOrWhiteSpace();
         
            return new PaymentReference
            {
                Value = paymentReference
            };
        }
    }
}