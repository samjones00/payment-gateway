namespace PaymentGateway.Domain.Models.Card
{
    public class CVV
    {
        public const int MinimumLength = 3;
        public const int MaximumLength = 4;

        public CVV(string value)
        {
            Value = value;
        }

        public string Value { get; init; }
    }
}