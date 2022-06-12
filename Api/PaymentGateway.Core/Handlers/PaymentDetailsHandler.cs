using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Core.Responses;
using PaymentGateway.Domain.Queries;

namespace PaymentGateway.Core.Handlers;
public class PaymentDetailsHandler : IRequestHandler<PaymentDetailsQuery, PaymentDetailsResponse>
{
    private readonly ILogger<ProcessPaymentHandler> _logger;

    public PaymentDetailsHandler(ILogger<ProcessPaymentHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PaymentDetailsResponse> Handle(PaymentDetailsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}