using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Core.Handlers;
public class PaymentDetailsHandler : IRequestHandler<PaymentDetailsQuery, PaymentDetailsResponse>
{
    private readonly ILogger<PaymentDetailsHandler> _logger;

    public PaymentDetailsHandler(ILogger<PaymentDetailsHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PaymentDetailsResponse> Handle(PaymentDetailsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}