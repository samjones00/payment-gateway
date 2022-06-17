using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models
{
    public class Amount
    {
        public decimal? Value { get; private set; }

        public static Amount Create(decimal paymentReference)
        {
            var result = new Amount
            {
                Value = paymentReference
            };

            var validationResult = new Validator().Validate(result);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                throw new InvalidMerchantException(validationResult.Errors.First().ErrorMessage);
            }

            return result;
        }

        private class Validator : AbstractValidator<Amount>
        {
            public Validator()
            {
                RuleFor(x => x.Value)
                    .NotEmpty()
                    .GreaterThan(0.0m)
                    .WithName(nameof(Amount));
            }
        }
    }
}
