namespace PaymentGateway.Domain.Models.Card
{
    public class PaymentCard
    {
        public CardNumber CardNumber { get; init; }

        public CVV CVV { get; init; }
        public ExpiryDate ExpiryDate { get; init; }
    }
}