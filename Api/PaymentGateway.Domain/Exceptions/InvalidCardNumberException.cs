using FluentValidation;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class InvalidCardNumberException : ValidationException
    {
        public InvalidCardNumberException(string? message) : base(message)
        {
        }
    }
}