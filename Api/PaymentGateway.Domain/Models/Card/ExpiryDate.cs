using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class ExpiryDate
    {
        public const string Month = nameof(Month);
        public const string Year = nameof(Year);

        public DateOnly Value { get; private set; }

        public static ExpiryDate Create(int month, int year)
        {
            month.ThrowIfZero();
            year.ThrowIfZero();

            return new ExpiryDate
            {
                Value = new DateOnly(year, month, 1)
            };
        }
    }
}