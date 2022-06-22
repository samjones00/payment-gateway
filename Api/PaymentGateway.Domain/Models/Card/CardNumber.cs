using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CardNumber
    {
        public const int MinimumLength = 14;
        public const int MaximumLength = 16;
        public const int UnmaskedDigitCount = 4;
        public const char MaskCharacter = 'X';

        private string value { get; init; } = string.Empty;

        public static CardNumber Create(string cardNumber)
        {
            cardNumber.ThrowIfNullOrWhiteSpace();

            return new CardNumber
            {
                value = cardNumber,
            };
        }

        public string Value => value;

        public static CardNumber Empty => new();
    }
}