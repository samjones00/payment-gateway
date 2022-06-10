namespace PaymentGateway.Domain.Models.Card
{
    public class PaymentCard
    {
        public CardNumber CardNumber { get; init; } = new CardNumber("");

        public CVV CVV { get; init; } = new CVV("");
    }
}