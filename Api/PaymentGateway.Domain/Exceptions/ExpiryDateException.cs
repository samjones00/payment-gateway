using System.Runtime.Serialization;
using FluentValidation;

namespace PaymentGateway.Domain.Exceptions
{
    [Serializable]
    public class ExpiryDateException : ValidationException
    {
        public ExpiryDateException(string? message) : base(message)
        {
        }
    }
}