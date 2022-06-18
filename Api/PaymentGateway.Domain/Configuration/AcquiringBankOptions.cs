namespace PaymentGateway.Domain.Configuration;
public class AcquiringBankOptions
{
    public const string SectionName = nameof(AcquiringBankOptions);

    public string BaseAddress { get; set; }
    public string PaymentEndpoint { get; set; }
}