using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models
{
    public class PaymentReference
    {
        public const int Length = 36;

        public string? Value { get; private set; }

        public static PaymentReference Create(string paymentReference)
        {
            var result = new PaymentReference
            {
                Value = paymentReference
            };

            var validationResult = new Validator().Validate(result);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                throw new InvalidPaymentReferenceException(validationResult.Errors.First().ErrorMessage);
            }

            return result;
        }

        private class Validator : AbstractValidator<PaymentReference>
        {
            public Validator()
            {
                RuleFor(x => x.Value)
                    .NotEmpty()
                    .Length(Length)
                    .WithName(nameof(PaymentReference));
            }
        }
    }
}