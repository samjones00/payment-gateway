using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Tests.Shared.Mocks
{
    public class MockBankConnectorService : IBankConnector
    {
        public Task<PaymentStatus> ProcessPayment(Payment payment, CancellationToken cancellationToken)
        {
            if (payment.PaymentReference.Value is Constants.UnsuccessfulPaymentReference)
            {
                return Task.FromResult(PaymentStatus.Unsuccessful);
            }

            return Task.FromResult(PaymentStatus.Successful);
        }
    }
}