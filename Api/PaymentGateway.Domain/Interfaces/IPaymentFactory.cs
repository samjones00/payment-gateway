using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentFactory
    {
        Payment Create(SubmitPaymentCommand command);
    }
}
