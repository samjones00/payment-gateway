using FluentValidation;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Validators
{
    public class SubmitPaymentCommandValidator : AbstractValidator<SubmitPaymentCommand>
    {
        private const string LettersAndSpacesOnly = "^[A-Za-z ]+$";
        private const string LettersOnly = "^[A-Za-z]+$";
        private const string NumbersOnly = "^[0-9]*$";

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitPaymentCommandValidator"/> class.
        /// </summary>
        /// <param name="dateTimeProvider">The date time provider.</param>
        public SubmitPaymentCommandValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.CardHolder)
                .Length(CardHolder.MinimumLength, CardHolder.MaximumLength)
                .Matches(LettersAndSpacesOnly);

            RuleFor(x => x.CardNumber)
                .Matches(NumbersOnly)
                .Length(CardNumber.MinimumLength, CardNumber.MaximumLength);

            RuleFor(x => x.CVV)
                .Length(CVV.MinimumLength, CVV.MaximumLength)
                .Matches(NumbersOnly);

            RuleFor(x => x.Currency)
               .Length(ISOCurrency.Length)
               .Matches(LettersOnly);

            RuleFor(x => x.ExpiryDateMonth)
                .InclusiveBetween(1, 12);

            RuleFor(x => x.ExpiryDateYear)
                .GreaterThanOrEqualTo(dateTimeProvider.UtcNow().Year);

            RuleFor(x => x.Amount)
                .GreaterThan(0.0m);

            RuleFor(x => x.PaymentReference)
                .NotNull()
                .NotEmpty()
                .Length(PaymentReference.Length);
        }
    }
}