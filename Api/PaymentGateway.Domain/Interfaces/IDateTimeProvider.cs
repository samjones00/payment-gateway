namespace PaymentGateway.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }
}
