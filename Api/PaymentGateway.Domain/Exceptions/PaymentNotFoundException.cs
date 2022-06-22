using System.Diagnostics.CodeAnalysis;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PaymentNotFoundException : Exception
    {
        public PaymentNotFoundException() { }

        public PaymentNotFoundException(string? message, string paymentReference, string merchantReference) : base(
            $"{message}. payment reference: {paymentReference}, merchant reference: {merchantReference}")
        {
        }
    }
}