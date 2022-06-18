using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IBankConnector
    {
        Task<PaymentStatus> Process(Payment payment, CancellationToken cancellationToken);
    }
}
