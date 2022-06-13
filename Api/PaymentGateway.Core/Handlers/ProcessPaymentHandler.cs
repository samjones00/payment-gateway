using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Core.Responses;
using PaymentGateway.Domain.Dto;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Handlers;
public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, ProcessPaymentResponse>
{
    private readonly ILogger<ProcessPaymentHandler> _logger;
    private readonly IBankConnectorService _bankConnectorService;
    private readonly IPaymentFactory _paymentFactory;

    public ProcessPaymentHandler(ILogger<ProcessPaymentHandler> logger, IBankConnectorService bankConnectorService, IPaymentFactory paymentFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bankConnectorService = bankConnectorService ?? throw new ArgumentNullException(nameof(bankConnectorService));
        _paymentFactory = paymentFactory ?? throw new ArgumentNullException(nameof(paymentFactory));
    }

    public async Task<ProcessPaymentResponse> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = _paymentFactory.Create(request);

        var bankResponse = await _bankConnectorService.Process(payment, cancellationToken);

        var response = new ProcessPaymentResponse
        {
            PaymentStatus = bankResponse.PaymentStatus,
            PaymentReference = bankResponse.PaymentReference.Value
        };

        return response;
    }
}