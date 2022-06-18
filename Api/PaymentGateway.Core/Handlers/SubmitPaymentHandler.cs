﻿using AutoMapper;
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
    private readonly IMapper _mapper;

    public SubmitPaymentHandler(IBankConnector bankConnectorService, ILogger<SubmitPaymentHandler> logger, IMapper mapper)
    {
        _bankConnectorService = bankConnectorService ?? throw new ArgumentNullException(nameof(bankConnectorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SubmitPaymentResponse> Handle(SubmitPaymentCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var payment = _mapper.Map<Payment>(command);

        var bankResponseStatus = await _bankConnectorService.Process(payment, cancellationToken);

        var response = new SubmitPaymentResponse
        {
            PaymentReference = command.PaymentReference,
            PaymentStatus = bankResponseStatus.ToString()
        };

        return response;
    }
}