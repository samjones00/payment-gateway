namespace PaymentGateway.AcquiringBank.CKO.Models
{
    public class Response
    {
        public string Status { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string OrderReference { get; set; } = string.Empty;
    }
}
