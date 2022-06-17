using MediatR;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Domain.Queries
{
    public class PaymentDetailsQuery : IRequest<PaymentDetailsResponse>
    {
        public string PaymentId { get; set; }
        public string MerchantId { get; set; }
    }
}