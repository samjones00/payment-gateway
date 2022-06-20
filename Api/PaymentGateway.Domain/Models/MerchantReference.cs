using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models
{
    public class MerchantReference
    {
        public string Value { get; private set; }

        public static MerchantReference Create(string merchantReference)
        {
            merchantReference.ThrowIfNullOrWhiteSpace();

            return new MerchantReference
            {
                Value = merchantReference
            };
        }
    }
}