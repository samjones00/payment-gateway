namespace PaymentGateway.AcquiringBank.CKO.Models
{
    public class PaymentCard
    {
        public string CardHolder { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public int ExpiryDateMonth { get; set; }
        public int ExpiryDateYear { get; set; }
    }
}
