using System.Diagnostics.CodeAnalysis;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PaymentAlreadyExistsException : Exception
    {
        public PaymentAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}