using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Models
{
    public class PaymentCard
    {
        public CardNumber CardNumber { get; init; }
        public CVV CVV { get; init; }
        public ExpiryDate ExpiryDate { get; set; }
        public CardHolder CardHolder { get; init; }
    }
}