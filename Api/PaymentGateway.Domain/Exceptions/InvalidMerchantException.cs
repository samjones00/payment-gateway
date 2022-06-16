using FluentValidation;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class InvalidMerchantException : ValidationException
    {
        public InvalidMerchantException(string? message) : base(message)
        {
        }
    }
}