namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class AcquiringBankException : Exception
    {
        public AcquiringBankException(string? message) : base(message)
        {
        }
    }
}