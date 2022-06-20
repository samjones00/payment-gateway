using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Models
{
    public class Payment
    {
        public Amount Amount { get; init; }

        public DateTime ProcessedOn { get; set; }

        public ISOCurrency Currency { get; init; }

        public MerchantReference MerchantReference { get; init; }

        public PaymentCard PaymentCard { get; init; }

        public PaymentReference PaymentReference { get; init; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}