using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class ExpiryDate
    {
        public const string Month = nameof(Month);
        public const string Year = nameof(Year);

        private DateOnly value { get; init; }

        public static ExpiryDate Create(int month, int year)
        {
            month.ThrowIfZero();
            year.ThrowIfZero();

            return new ExpiryDate
            {
                value = new DateOnly(year, month, 1)
            };
        }

        public DateOnly Value => value;

        public static ExpiryDate Empty => new();    
    }
}