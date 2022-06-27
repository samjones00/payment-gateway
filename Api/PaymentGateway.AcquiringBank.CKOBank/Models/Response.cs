namespace PaymentGateway.AcquiringBank.CKO.Models
{
    public record struct Response
    {
        public string? Status { get; set; }
        public string? OrderReference { get; set; }
    }
}
