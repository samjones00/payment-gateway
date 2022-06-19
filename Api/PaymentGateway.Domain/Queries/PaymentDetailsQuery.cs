using MediatR;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Domain.Queries
{
    public class PaymentDetailsQuery : IRequest<PaymentDetailsResponse>
    {
        public string MerchantReference { get; }
        public string PaymentReference { get; }

        public PaymentDetailsQuery(string merchantReference, string paymentReference)
        {
            MerchantReference = merchantReference;
            PaymentReference = paymentReference;
        }
    }
}