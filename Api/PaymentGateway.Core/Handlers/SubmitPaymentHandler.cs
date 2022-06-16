using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Core.Responses;
using PaymentGateway.Domain.Dto;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Handlers;
public class SubmitPaymentHandler : IRequestHandler<SubmitPaymentCommand, SubmitPaymentResponse>
{
    private readonly ILogger<SubmitPaymentHandler> _logger;
    private readonly IBankConnectorService _bankConnectorService;
    private readonly IMapper _mapper;

    public SubmitPaymentHandler(ILogger<SubmitPaymentHandler> logger, IBankConnectorService bankConnectorService, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bankConnectorService = bankConnectorService ?? throw new ArgumentNullException(nameof(bankConnectorService));
        _mapper = mapper;
    }

    public async Task<SubmitPaymentResponse> Handle(SubmitPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = _mapper.Map<Payment>(request);

        var bankResponse = await _bankConnectorService.Process(payment, cancellationToken);

        var response = new SubmitPaymentResponse
        {
            PaymentStatus = bankResponse.PaymentStatus
        };

        return response;
    }
}