using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
