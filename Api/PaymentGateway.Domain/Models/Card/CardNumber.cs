using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CardNumber
    {
        public const int MinimumLength = 14;
        public const int MaximumLength = 16;
        public const int UnmaskedDigitCount = 4;
        public const char MaskCharacter = 'X';

        public string? Value { get; private set; }
        public string? MaskedValue { get; private set; }


        public static CardNumber Create(string cardNumber)
        {
            var result = new CardNumber
            {
                Value = cardNumber,
                MaskedValue = MaskString(cardNumber)
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

        public static string MaskString(string source)
        {
            if (string.IsNullOrEmpty(source) || UnmaskedDigitCount >= source.Length)
            {
                return source;
            }

            string? masked = new(MaskCharacter, source.Length - UnmaskedDigitCount);
            var notMasked = source.Remove(0, source.Length - UnmaskedDigitCount);

            return $"{masked}{notMasked}";
        }
    }
}