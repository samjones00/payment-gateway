using MediatR;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Domain.Queries
{
    public class PaymentDetailsQuery : IRequest<PaymentDetailsResponse>
    {
        public string MerchantReference { get; set; }
        public string PaymentReference { get; set; }

        public PaymentDetailsQuery(string merchantReference, string paymentReference)
        {
            MerchantReference = merchantReference;
            PaymentReference = paymentReference;
        }
    }
}