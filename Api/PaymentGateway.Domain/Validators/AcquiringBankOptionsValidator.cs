using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using PaymentGateway.Domain.Configuration;

namespace PaymentGateway.Domain.Validators
{
    [ExcludeFromCodeCoverage]
    public class AcquiringBankOptionsValidator : AbstractValidator<AcquiringBankOptions>
    {
        public AcquiringBankOptionsValidator()
        {
            RuleFor(x => x.BaseAddress)
                .NotEmpty();

            RuleFor(x => x.RetryWaitTimeInSeconds)
                .NotEmpty();

            RuleFor(x => x.RetryCount)
                .NotEmpty();

            RuleFor(x => x.PaymentEndpoint)
                .NotEmpty();
        }
    }
}