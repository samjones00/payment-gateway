using System.Diagnostics.CodeAnalysis;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PaymentNotFoundException : Exception
    {
        private string paymentReference;
        private string merchantReference;

        public PaymentNotFoundException()
        {

        }

        public PaymentNotFoundException(string? message, string paymentReference, string merchantReference) : base(message)
        {
            this.paymentReference = paymentReference;
            this.merchantReference = merchantReference;
        }
    }
}