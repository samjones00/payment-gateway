namespace PaymentGateway.AcquiringBanks.CKO.Models
{
    public class Request
    {
        public string PaymentReference { get; set; }
        public string MerchantReference { get; set; }
        public string Currency { get; set; }
        public PaymentCard PaymentCard { get; set; }
    }
}