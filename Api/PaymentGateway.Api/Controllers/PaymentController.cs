using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentService _paymentService;

    public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
    {
        _logger = logger;
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _paymentService.Process();

        return Ok();
    }
}
