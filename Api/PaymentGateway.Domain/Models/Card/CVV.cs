using PaymentGateway.Domain.Extensions;

namespace PaymentGateway.Domain.Models.Card
{
    public class CVV
    {
        public const int MinimumLength = 3;
        public const int MaximumLength = 4;

        private string value { get; init; } = string.Empty;

        public static CVV Create(string cvv)
        {
            cvv.ThrowIfNullOrWhiteSpace();

            return new CVV
            {
                value = cvv
            };
        }

        public string Value => value;

        public static CVV Empty => new();
    }
}