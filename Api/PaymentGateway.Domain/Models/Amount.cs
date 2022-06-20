namespace PaymentGateway.Domain.Models
{
    public class Amount
    {
        public decimal Value { get; private set; }

        public static Amount Create(decimal amount)
        {
            return new Amount
            {
                Value = amount
            };
        }
    }
}