using FluentValidation;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Validators
{
    public class PaymentCardValidator : AbstractValidator<PaymentCard>
    {
        public PaymentCardValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty();
            RuleFor(x => x.CVV).NotEmpty();

            RuleFor(x => x.CardNumber.Value)
                .NotEmpty()
                .MinimumLength(CardNumber.MinimumLength)
                .WithMessage($"Card number must be at least {CardNumber.MinimumLength} digits.")
                .MaximumLength(CardNumber.MaximumLength)
                .WithMessage($"Card number must be a maximum of {CardNumber.MaximumLength} digits.");

            RuleFor(x => x.CVV.Value)
               .NotEmpty()
               .MinimumLength(CVV.MinimumLength)
               .WithMessage($"CVV number must be at least {CVV.MinimumLength} digits.")
               .MaximumLength(CVV.MaximumLength)
               .WithMessage($"CVV number must be a maximum of {CVV.MaximumLength} digits.");
        }
    }
}
