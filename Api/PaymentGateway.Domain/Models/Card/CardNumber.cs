namespace PaymentGateway.Domain.Models.Card
{
    public class CardNumber
    {
        public const int MinimumLength = 14;
        public const int MaximumLength = 16;
        public const int UnmaskedDigitCount = 4;
        public const char MaskCharacter = 'X';

        public string Value { get; private set; }

        public static CardNumber Create(string cardNumber)
        {
            var result = new CardNumber
            {
                Value = cardNumber,
            };

            return result;
        }
    }
}