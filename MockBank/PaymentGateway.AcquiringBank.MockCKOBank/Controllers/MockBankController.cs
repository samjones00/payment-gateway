using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentGateway.AcquiringBank.MockCKOBank.Models;

namespace PaymentGateway.AcquiringBank.MockCKOBank.Controllers;

[ApiController]
public class MockBankController : ControllerBase
{
    private readonly ILogger<MockBankController> _logger;

    public MockBankController(ILogger<MockBankController> logger)
    {
        _logger = logger;
    }

    [HttpPost(nameof(Process))]
    [ProducesResponseType(typeof(MockResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Process(MockRequest request)
    {
        _logger.LogInformation($"Processing payment: {JsonConvert.SerializeObject(request)}");

        switch (request.PaymentReference)
        {
            case "ba1c9df4-001e-4922-9efa-488b59850bc4":
                _logger.LogInformation("BadRequest.");
                return BadRequest(new MockResponse(request.PaymentReference, "Unsuccessful"));
            default:
                break;
        }
        _logger.LogInformation("Accepted.");
        _logger.LogInformation(new string('*', 60));

        return Accepted(new MockResponse(request.PaymentReference, "Successful"));
    }
}