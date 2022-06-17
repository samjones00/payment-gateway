using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CardNumber
    {
        public const int MinimumLength = 14;
        public const int MaximumLength = 16;
        public const int UnmaskedDigitCount = 4;

        public string? Value { get; private set; }
        public string? MaskedValue { get; private set; }


        public static CardNumber Create(string cardNumber)
        {
            var result = new CardNumber
            {
                Value = cardNumber,
                MaskedValue = GetMaskedValue(cardNumber)
            };

            var validationResult = new Validator().Validate(result);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                throw new InvalidCardNumberException(validationResult.Errors.First().ErrorMessage);
            }

            return result;
        }

        private class Validator : AbstractValidator<CardNumber>
        {
            public Validator()
            {
                RuleFor(x => x.Value)
                    .NotEmpty()
                    //.CreditCard()
                    .Length(MinimumLength, MaximumLength)
                    .WithName(nameof(CardNumber));
            }
        }

        private static string? GetMaskedValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var digitsToMask = value.Length - UnmaskedDigitCount;
                var lastDigits = value.Substring(digitsToMask);
                var masked = new string('*', digitsToMask);
                return string.Concat(masked, lastDigits); ;
            }

            return null;
        }
    }
}