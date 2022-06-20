using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CardHolder
    {
        public const int MinimumLength = 2;
        public const int MaximumLength = 40;

        public string Value { get; private set; }

        public static CardHolder Create(string cardHolder)
        {
            cardHolder.ThrowIfNullOrWhiteSpace();

            return new CardHolder
            {
                Value = cardHolder
            };
        }
    }
}