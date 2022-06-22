using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CardHolder
    {
        public const int MinimumLength = 2;
        public const int MaximumLength = 40;

        private string value { get; init; } = string.Empty;

        public static CardHolder Create(string cardHolder)
        {
            cardHolder.ThrowIfNullOrWhiteSpace();

            return new CardHolder
            {
                value = cardHolder
            };
        }

        public string Value => value;

        public static CardHolder Empty => new();
    }
}