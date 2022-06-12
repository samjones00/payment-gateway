using System.Net;
using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Core.Responses;
using PaymentGateway.Domain.Dto;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Queries;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IMediator _mediator;

    public PaymentController(ILogger<PaymentController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProcessPaymentResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.ValidationErrors.Any())
        {
            return BadRequest(response);
        }

        return Created($"/details/{response.PaymentReference}", response);
    }


    [HttpGet]
    [ProducesResponseType(typeof(PaymentDetailsResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPaymentDetails(PaymentDetailsQuery query)
    {
        throw new NotImplementedException();
        //var response = await _processPaymentService.Process(request);

        //if (response.ValidationErrors.Any())
        //{
        //    return BadRequest(response);
        //}

        //return Created($"/details/{response.PaymentReference}", response);
    }
}
