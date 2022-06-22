namespace PaymentGateway.AcquiringBank.CKO.Models
{
    public class Request
    {
        public string PaymentReference { get; set; } = string.Empty;
        public string MerchantReference { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public PaymentCard PaymentCard { get; set; } = new();
    }
}