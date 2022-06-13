using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class InvalidPaymentReferenceException : Exception
    {
        public InvalidPaymentReferenceException()
        {
        }

        public InvalidPaymentReferenceException(string? message) : base(message)
        {
        }

        public InvalidPaymentReferenceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidPaymentReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}