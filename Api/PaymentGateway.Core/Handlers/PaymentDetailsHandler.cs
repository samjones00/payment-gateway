using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Core.Handlers;
public class PaymentDetailsHandler : IRequestHandler<PaymentDetailsQuery, PaymentDetailsResponse>
{
    private readonly ILogger<PaymentDetailsHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Payment> _repository;

    public PaymentDetailsHandler(ILogger<PaymentDetailsHandler> logger, IMapper mapper, IRepository<Payment> repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PaymentDetailsResponse> Handle(PaymentDetailsQuery query, CancellationToken cancellationToken)
    {
        var payment = _repository.Get(query.MerchantReference, query.PaymentReference);

        var details = _mapper.Map<PaymentDetailsResponse>(payment);

        return details;
    }
}