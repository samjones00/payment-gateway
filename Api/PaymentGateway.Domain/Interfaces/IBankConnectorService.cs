using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IBankConnectorService
    {
        Task<Payment> Process(Payment payment, CancellationToken cancellationToken);
    }
}
