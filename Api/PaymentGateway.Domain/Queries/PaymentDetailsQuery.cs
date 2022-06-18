using MediatR;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Domain.Queries
{
    public class PaymentDetailsQuery : IRequest<PaymentDetailsResponse>
    {
        public string PaymentReference { get; set; }
        public string MerchantReference { get; set; }
    }
}