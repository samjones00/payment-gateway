using MediatR;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Domain.Commands
{
    public record SubmitPaymentCommand : IRequest<SubmitPaymentResponse>
    {
        public decimal Amount { get; init; }
        public int ExpiryDateMonth { get; init; }
        public int ExpiryDateYear { get; init; }
        public string CardHolder { get; init; }
        public string CardNumber { get; init; }
        public string Currency { get; init; }
        public string CVV { get; init; }
        public string MerchantReference { get; init; }
        public string PaymentReference { get; init; }
    }
}