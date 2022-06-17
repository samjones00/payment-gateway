

using MediatR;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Domain.Commands
{
    public class SubmitPaymentCommand : IRequest<SubmitPaymentResponse>
    {
        public string PaymentReference { get; set; }
        public string MerchantReference { get; set; }
        public string ShopperReference { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public int ExpiryDateYear { get; set; }
        public int ExpiryDateMonth { get; set; }
        public string CardHolder { get; set; }
    }
}
