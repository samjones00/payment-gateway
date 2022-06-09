using FluentValidation;
using PaymentGateway.Domain.Configuration;

namespace PaymentGateway.Domain.Validators
{
    public class GatewayOptionsValidator : AbstractValidator<GatewayOptions>
    {
        public GatewayOptionsValidator()
        {
            RuleFor(x => x.BaseAddress).NotEmpty();
        }
    }
}
