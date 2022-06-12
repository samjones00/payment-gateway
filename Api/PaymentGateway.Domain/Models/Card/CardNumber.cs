namespace PaymentGateway.Domain.Models.Card
{
    public class CardNumber
    {
        public const int MinimumLength = 14;
        public const int MaximumLength = 16;

        public string Value { get; init; }

        public CardNumber(string value)
        {
            Value = value;
        }
    }
}