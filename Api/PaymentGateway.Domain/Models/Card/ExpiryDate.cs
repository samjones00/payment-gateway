using FluentValidation;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.Models.Card
{
    public class ExpiryDate
    {
        public const string Month = nameof(Month);
        public const string Year = nameof(Year);

        public DateOnly Value { get; private set; }
        public static DateOnly CurrentDate { get; private set; }

        public static ExpiryDate Create(int month, int year, DateOnly currentDate)
        {
            CurrentDate = currentDate;

            var result = new ExpiryDate
            {
                Value = new DateOnly(year, month, 1)
            };

            var validationResult = new Validator().Validate(result);

            if (!validationResult.IsValid && validationResult.Errors.Any())
            {
                throw new ExpiryDateException(validationResult.Errors.First().ErrorMessage);
            }

            return result;
        }

        private class Validator : AbstractValidator<ExpiryDate>
        {
            public Validator()
            {
                RuleFor(x => x.Value.Month)
                    .InclusiveBetween(1, 12)
                    .WithName(nameof(Month));

                RuleFor(x => x.Value.Year)
                   .GreaterThanOrEqualTo(CurrentDate.Year)
                   .WithName(nameof(Year));
            }
        }
    }
}