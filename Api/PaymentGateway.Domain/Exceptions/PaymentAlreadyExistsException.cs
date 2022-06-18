using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class PaymentAlreadyExistsException : Exception
    {
        private string paymentReference;
        private string merchantReference;

        public PaymentAlreadyExistsException(string? message, string paymentReference, string merchantReference) : base(message)
        {
            this.paymentReference = paymentReference;
            this.merchantReference = merchantReference;
        }
    }
}