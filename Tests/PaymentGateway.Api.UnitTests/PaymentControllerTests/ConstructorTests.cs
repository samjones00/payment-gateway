using PaymentGateway.Api.Controllers;
using PaymentGateway.Tests.Shared;

namespace PaymentControllerTests;

public class ConstructorTests : TestBase
{
    [Test]
    public void Constructor()
    {
        RunGuardClauseChecks<PaymentController>();
    }
}