namespace PaymentGateway.Domain.Configuration;
public class AcquiringBankOptions
{
    public const string SectionName = nameof(AcquiringBankOptions);

    public string BaseAddress { get; set; } = string.Empty;
    public string PaymentEndpoint { get; set; } = string.Empty;
    public int RetryCount { get; set; }
    public int RetryWaitTimeInSeconds { get; set; }
}