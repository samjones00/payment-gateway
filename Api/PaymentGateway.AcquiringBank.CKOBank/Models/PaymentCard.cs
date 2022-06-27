namespace PaymentGateway.AcquiringBank.CKO.Models
{
    public record struct PaymentCard
    {
        public string? CardHolder { get; init; } 
        public string? CardNumber { get; init; } 
        public string? CVV { get; init; }
        public int? ExpiryDateMonth { get; init; }
        public int? ExpiryDateYear { get; init; }
    }
}
