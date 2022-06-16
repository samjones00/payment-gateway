using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CardNumber
    {
        public const int MinimumLength = 14;
        public const int MaximumLength = 16;

        public string? Value { get; private set; }

        public static CardNumber Create(string cardNumber)
        {
            var result = new CardNumber
            {
                Value = cardNumber
            };

            var validationResult = new Validator().Validate(result);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                throw new InvalidCardNumberException(validationResult.Errors.First().ErrorMessage);
            }

            return result;
        }

        public static CardNumber Empty() => Create(new string('0', MinimumLength));

        private class Validator : AbstractValidator<CardNumber>
        {
            public Validator()
            {
                RuleFor(x => x.Value)
                    .NotEmpty()
                    .CreditCard()
                    .Length(MinimumLength, MaximumLength)
                    .WithName(nameof(CardNumber));
            }
        }
    }
}