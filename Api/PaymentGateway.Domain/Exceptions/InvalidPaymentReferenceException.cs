using System.Runtime.Serialization;
using FluentValidation;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class InvalidPaymentReferenceException : ValidationException
    {
        public InvalidPaymentReferenceException(string? message) : base(message)
        {
        }
    }
}