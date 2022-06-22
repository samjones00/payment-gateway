using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Models
{
    public class PaymentCard
    {
        public CardNumber CardNumber { get; set; } = CardNumber.Empty;
        public CVV CVV { get; init; } = CVV.Empty;
        public ExpiryDate ExpiryDate { get; set; } = ExpiryDate.Empty;
        public CardHolder CardHolder { get; init; } = CardHolder.Empty;

        public static PaymentCard Empty => new()
        {
            CardNumber = CardNumber.Empty,
            CVV = CVV.Empty,
            ExpiryDate = ExpiryDate.Empty,
            CardHolder = CardHolder.Empty
        };
    }
}