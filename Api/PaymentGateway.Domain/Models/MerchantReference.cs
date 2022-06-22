using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models
{
    public class MerchantReference
    {
        private string value { get; init; } = string.Empty;

        public static MerchantReference Create(string merchantReference)
        {
            merchantReference.ThrowIfNullOrWhiteSpace();

            return new MerchantReference
            {
                value = merchantReference
            };
        }

        public string Value => value;
        
        public static MerchantReference Empty => new();
    }
}