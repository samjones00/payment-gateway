namespace PaymentGateway.Domain.Models
{
    public class Amount
    {
        private decimal value { get; init; }

        public static Amount Create(decimal amount)
        {
            return new Amount
            {
                value = amount
            };
        }

        public decimal Value => value;

        public static Amount Empty => new();
    }
}