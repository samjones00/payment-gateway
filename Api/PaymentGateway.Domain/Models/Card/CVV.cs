using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CVV
    {
        public const int MinimumLength = 3;
        public const int MaximumLength = 4;

        public string Value { get; private set; }

        public static CVV Create(string cvv)
        {
            cvv.ThrowIfNullOrWhiteSpace();

            return new CVV
            {
                Value = cvv
            };
        }
    }
}