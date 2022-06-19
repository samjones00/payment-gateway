namespace PaymentGateway.Domain.Models.Card
{
    public class CardNumber
    {
        public const int MinimumLength = 14;
        public const int MaximumLength = 16;
        public const int UnmaskedDigitCount = 4;
        public const char MaskCharacter = 'X';

        public string? Value { get; private set; }


        public static CardNumber Create(string cardNumber)
        {
            var result = new CardNumber
            {
                Value = cardNumber,
            };

            //var validationResult = new Validator().Validate(result);

            //if (!validationResult.IsValid && validationResult.Errors.Any())
            //{
            //    throw new InvalidCardNumberException(validationResult.Errors.First().ErrorMessage);
            //}

            return result;
        }

        //private class Validator : AbstractValidator<CardNumber>
        //{
        //    public Validator()
        //    {
        //        RuleFor(x => x.Value)
        //            .NotEmpty()
        //            //.CreditCard()
        //            .Length(MinimumLength, MaximumLength)
        //            .WithName(nameof(CardNumber));
        //    }
        //}

    }
    public static class CardNumberExtensions
    {
        public static string ToMaskedValue(this CardNumber source)
        {
            if (string.IsNullOrEmpty(source.Value) || CardNumber.UnmaskedDigitCount >= source.Value.Length)
            {
                return string.Empty;
            }

            string? masked = new(CardNumber.MaskCharacter, source.Value.Length - CardNumber.UnmaskedDigitCount);
            var notMasked = source.Value.Remove(0, source.Value.Length - CardNumber.UnmaskedDigitCount);

            return $"{masked}{notMasked}";
        }
    }
}