namespace PaymentGateway.AcquiringBank.CKO.Models
{
    public class PaymentCard
    {
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public int ExpiryDateMonth { get; set; }
        public int ExpiryDateYear { get; set; }
    }
}
