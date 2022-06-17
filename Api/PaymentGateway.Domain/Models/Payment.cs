using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Models
{
    public class Payment
    {
        public ShopperReference PaymentReference { get; set; }

        public Merchant Merchant { get; init; }

        public ShopperReference ShopperReference { get; init; }

        public PaymentCard PaymentCard { get; init; }
        public Amount Amount { get; init; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
