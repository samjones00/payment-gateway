using System.Runtime.Serialization;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class PaymentNotFoundException : Exception
    {
        private string paymentReference;
        private string merchantReference;

        public PaymentNotFoundException(string? message, string paymentReference, string merchantReference) : base(message)
        {
            this.paymentReference = paymentReference;
            this.merchantReference = merchantReference;
        }
    }
}