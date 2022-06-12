using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Models
{
    public class Payment
    {
        public PaymentReference PaymentReference { get; set; }

        public Merchant Merchant { get; init; }

        public Shopper Shopper { get; init; }

        public PaymentCard PaymentCard { get; init; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
