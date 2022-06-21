using System.Diagnostics.CodeAnalysis;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AcquiringBankException : Exception
    {
        public AcquiringBankException(string? message) : base(message)
        {
        }
    }
}