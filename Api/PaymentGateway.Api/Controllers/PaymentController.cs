using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Extensions;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Route(ApiRoutes.Path)]
[Produces(MediaTypeNames.Application.Json)]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PaymentController(ILogger<PaymentController> logger, IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    [HttpPost(ApiRoutes.SubmitPayment)]
    [ProducesResponseType(typeof(SubmitPaymentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProcessPayment(SubmitPaymentCommand command)
    {
        var merchantReference = _httpContextAccessor.GetMerchantReference();
        command.MerchantReference = merchantReference;
        var response = await _mediator.Send(command);

        var r = response.PaymentStatus;

        return CreatedAtAction(nameof(GetPaymentDetails), new { response.PaymentReference }, response);
    }


    [HttpGet(ApiRoutes.GetPaymentDetails)]
    [ProducesResponseType(typeof(PaymentDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentDetails(string paymentReference)
    {
        var merchantReference = _httpContextAccessor.GetMerchantReference();
        var response = await _mediator.Send(new PaymentDetailsQuery(merchantReference, paymentReference));

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }
}
