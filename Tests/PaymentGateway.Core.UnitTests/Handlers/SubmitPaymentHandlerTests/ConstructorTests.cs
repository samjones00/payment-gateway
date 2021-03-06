using PaymentGateway.AcquiringBank.CKO.Models;
using PaymentGateway.Core.Services;
using PaymentGateway.Tests.Shared;

namespace SubmitPaymentHandlerTests;

public class ConstructorTests : TestBase
{
    [Test]
    public void Constructor()
    {
        RunGuardClauseChecks<BankConnectorService<Request, Response>>();
    }
}