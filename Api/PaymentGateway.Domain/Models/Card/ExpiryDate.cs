using FluentValidation;

namespace PaymentGateway.Domain.Models.Card
{
    public class ExpiryDate
    {
        public const string Month = nameof(Month);
        public const string Year = nameof(Year);

        public DateOnly Value { get; private set; }
        public static DateOnly CurrentDate { get; private set; }

        public static ExpiryDate Create(int month, int year)
        {
            var result = new ExpiryDate
            {
                Value = new DateOnly(year, month, 1)
            };

            return result;
        }
    }
}