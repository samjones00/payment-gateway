using System.Diagnostics.CodeAnalysis;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class PaymentAlreadyExistsException : Exception
    {
        public PaymentAlreadyExistsException(string? message, string paymentReference, string merchantReference) : base(
            $"{message}, payment reference: {paymentReference}, merchant reference: {merchantReference}")
        {
        }
    }
}