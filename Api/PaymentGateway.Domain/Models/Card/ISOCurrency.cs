using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class ISOCurrency
    {
        public const int Length = 3;

        private string value { get; init; } = string.Empty;

        public static ISOCurrency Create(string currency)
        {
            currency.ThrowIfNullOrWhiteSpace();

            return new ISOCurrency
            {
                value = currency.ToUpper(),
            };
        }

        public string Value => value;

        public static ISOCurrency Empty => new();
    }
}