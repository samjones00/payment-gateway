using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Core.Handlers;
public class PaymentDetailsHandler : IRequestHandler<PaymentDetailsQuery, PaymentDetailsResponse>
{
    private readonly ILogger<PaymentDetailsHandler> _logger;
    private readonly IRepository<Payment> _repository;

    public PaymentDetailsHandler(ILogger<PaymentDetailsHandler> logger, IRepository<Payment> repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PaymentDetailsResponse> Handle(PaymentDetailsQuery query, CancellationToken cancellationToken)
    {
        var payment = _repository.Get(query.MerchantReference, query.PaymentReference);

        return new PaymentDetailsResponse
        {
            CardNumber = payment.PaymentCard.CardNumber.GetMaskedValue(),
            Amount = payment.Amount.Value,
            Status = payment.PaymentStatus.ToString()
        };
    }
}