using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class ExpiryDateException : Exception
    {
        public ExpiryDateException()
        {
        }

        public ExpiryDateException(string? message) : base(message)
        {
        }

        public ExpiryDateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ExpiryDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}