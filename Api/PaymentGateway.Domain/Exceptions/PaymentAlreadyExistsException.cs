using System.Diagnostics.CodeAnalysis;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
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