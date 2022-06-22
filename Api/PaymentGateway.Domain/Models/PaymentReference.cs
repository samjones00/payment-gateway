using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models
{
    public class PaymentReference
    {
        public const int Length = 36;

        private string value { get; init; } = string.Empty;

        public static PaymentReference Create(string paymentReference)
        {
            paymentReference.ThrowIfNullOrWhiteSpace();

            return new PaymentReference
            {
                value = paymentReference
            };
        }

        public string Value => value;

        public static PaymentReference Empty => new();
    }
}