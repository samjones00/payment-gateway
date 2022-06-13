using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CVV
    {
        public const int MinimumLength = 3;
        public const int MaximumLength = 4;

        public string? Value { get; private set; }

        public static CVV Create(string cardNumber)
        {
            var result = new CVV
            {
                Value = cardNumber
            };

            var validationResult = new Validator().Validate(result);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                throw new InvalidCVVException(validationResult.Errors.First().ErrorMessage);
            }

            return result;
        }

        public static CVV Empty() => new();

        private class Validator : AbstractValidator<CVV>
        {
            public Validator()
            {
                RuleFor(x => x.Value)
                    .NotEmpty()
                    .Length(MinimumLength, MaximumLength)
                    .WithName(nameof(CVV));
            }
        }
    }
}