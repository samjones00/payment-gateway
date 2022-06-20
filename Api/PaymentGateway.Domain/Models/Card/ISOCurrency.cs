using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class ISOCurrency
    {
        public const int Length = 3;

        public string Value { get; private set; }

        public static ISOCurrency Create(string currency)
        {
            currency.ThrowIfNullOrWhiteSpace();

            return new ISOCurrency
            {
                Value = currency.ToUpper(),
            };
        }
    }
}