using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models
{
    public class Merchant
    {
        public string? Value { get; private set; }

        public static Merchant Create(string paymentReference)
        {
            var result = new Merchant
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

        private class Validator : AbstractValidator<Merchant>
        {
            public Validator()
            {
                RuleFor(x => x.Value)
                    .NotEmpty()
                    .WithMessage($"{nameof(Merchant)} cannot be empty.");
            }
        }
    }
}
