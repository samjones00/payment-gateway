namespace PaymentGateway.Domain.Models
{
    public class MerchantReference
    {
        public string Value { get; private set; }

        public static MerchantReference Create(string merchantReference)
        {
            var result = new MerchantReference
            {
                Value = merchantReference
            };

            return result;
        }
    }
}