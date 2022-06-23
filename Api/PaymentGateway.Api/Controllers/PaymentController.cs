using System.Net.Mime;
using System.Security.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Constants;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Extensions;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Api.Controllers;

/// <summary>
/// The controller responsible for payments
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
[ApiController]
[Route(ApiRoutes.Path)]
[Produces(MediaTypeNames.Application.Json)]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentController"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    /// <param name="httpContextAccessor"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public PaymentController(ILogger<PaymentController> logger, IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Submits the payment.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.SubmitPayment)]
    [ProducesResponseType(typeof(SubmitPaymentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
    public async Task<IActionResult> SubmitPayment(SubmitPaymentCommand command)
    {
        return await Handle(async () =>
        {
            ArgumentNullException.ThrowIfNull(command);

            var merchantReference = _httpContextAccessor.GetMerchantReference();
            command.MerchantReference = merchantReference;
            var response = await _mediator.Send(command);

            return response.PaymentStatus == PaymentStatus.Successful.ToString()
                ? CreatedAtAction(nameof(GetPaymentDetails), new { response.PaymentReference }, response)
                : BadRequest(response);
        });
    }

    /// <summary>
    /// Gets the payment details.
    /// </summary>
    /// <param name="paymentReference">The payment reference.</param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.GetPaymentDetails)]
    [ProducesResponseType(typeof(PaymentDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentDetails(string paymentReference)
    {
        return await Handle(async () =>
        {
            paymentReference.ThrowIfNullOrWhiteSpace();

            var response = await _mediator.Send(new PaymentDetailsQuery(_httpContextAccessor.GetMerchantReference(), paymentReference));
            return Ok(response);
        });
    }

    private async Task<IActionResult> Handle(Func<Task<IActionResult>> func)
    {
        try
        {
            return await func.Invoke();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (PaymentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (PaymentAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (AcquiringBankException)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(StatusCodes.Status504GatewayTimeout);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}