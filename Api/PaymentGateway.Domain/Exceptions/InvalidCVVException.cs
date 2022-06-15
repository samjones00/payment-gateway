using System.Runtime.Serialization;
using FluentValidation;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class InvalidCVVException : ValidationException
    {
        public InvalidCVVException(string? message) : base(message)
        {
        }
    }
}