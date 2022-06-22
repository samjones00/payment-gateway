using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Models
{
    public class Payment
    {
        public Amount Amount { get; init; } = Amount.Empty;

        public DateTime ProcessedOn { get; set; } = new();

        public ISOCurrency Currency { get; init; } = ISOCurrency.Empty;

        public MerchantReference MerchantReference { get; init; } = MerchantReference.Empty;

        public PaymentCard PaymentCard { get; init; } = PaymentCard.Empty;

        public PaymentReference PaymentReference { get; init; } = PaymentReference.Empty;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.None;
    }
}