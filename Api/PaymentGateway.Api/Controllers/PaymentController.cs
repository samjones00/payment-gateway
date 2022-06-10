using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IPaymentService _paymentService;

    public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    }

    [HttpGet]
    //[ProducesResponseType(200, Type = typeof(GetPaymentResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get()
    {
        await _paymentService.Process();

        return Ok();
    }
}
