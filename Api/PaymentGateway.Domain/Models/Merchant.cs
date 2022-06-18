namespace PaymentGateway.Domain.Models
{
    public class Merchant
    {
        public string? Value { get; private set; }

        public static Merchant Create(string merchantReference)
        {
            var result = new Merchant
            {
                Value = merchantReference
            };

            return result;
        }
    }
}