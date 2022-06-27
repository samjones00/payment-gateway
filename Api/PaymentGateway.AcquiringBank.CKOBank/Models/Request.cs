namespace PaymentGateway.AcquiringBank.CKO.Models
{
    public record struct Request
    {
        public string? PaymentReference { get; init; }
        public string? MerchantReference { get; init; } 
        public string? Currency { get; init; } 
        public PaymentCard? PaymentCard { get; init; } 
    }
}