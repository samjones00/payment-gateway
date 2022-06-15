using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Api.IntegrationTests
{
    public class MockBankConnectorService : IBankConnectorService
    {
        public Task<Payment> Process(Payment payment, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
