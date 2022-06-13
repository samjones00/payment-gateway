using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class InvalidCVVException : Exception
    {
        public InvalidCVVException(string? message) : base(message)
        {
        }
    }
}