using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class InvalidCardNumberException : Exception
    {
        public InvalidCardNumberException()
        {
        }

        public InvalidCardNumberException(string? message) : base(message)
        {
        }

        public InvalidCardNumberException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidCardNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}