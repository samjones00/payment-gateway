using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Tests.Shared
{
    public class MockBankConnectorService : IBankConnector
    {
        public Task<PaymentStatus> Process(Payment payment, CancellationToken cancellationToken)
        {
            return Task.FromResult(PaymentStatus.Successful);
        }
    }
}
