using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Api.IntegrationTests
{
    public class MockBankConnectorService : IBankConnectorService
    {
        public Task<Payment> Process(Payment payment, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Payment
            {
                PaymentStatus = Domain.Enums.PaymentStatus.Successful
            });
        }
    }
}
