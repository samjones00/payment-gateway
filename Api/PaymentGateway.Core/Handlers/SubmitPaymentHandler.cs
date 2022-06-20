using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Core.Handlers;

public class SubmitPaymentHandler : IRequestHandler<SubmitPaymentCommand, SubmitPaymentResponse>
{
    private readonly ILogger<SubmitPaymentHandler> _logger;
    private readonly IBankConnector _bankConnectorService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;
    private readonly IRepository<Payment> _repository;

    public SubmitPaymentHandler(
        IBankConnector bankConnectorService,
        IDateTimeProvider dateTimeProvider,
        ILogger<SubmitPaymentHandler> logger,
        IMapper mapper,
        IRepository<Payment> repository)
    {
        _bankConnectorService = bankConnectorService ?? throw new ArgumentNullException(nameof(bankConnectorService));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository;
    }

    public async Task<SubmitPaymentResponse> Handle(SubmitPaymentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var payment = _mapper.Map<Payment>(command);

        _repository.Insert(payment);

        var bankResponseStatus = await _bankConnectorService.Process(payment, cancellationToken);

        payment.PaymentStatus = bankResponseStatus;
        payment.ProcessedOn = _dateTimeProvider.UtcNow();
        _repository.Update(payment);

        var result = _mapper.Map<SubmitPaymentResponse>(payment);

        return result;
    }
}