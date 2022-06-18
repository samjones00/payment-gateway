namespace PaymentGateway.Domain.Configuration;
public class AcquiringBankOptions
{
    public string BaseAddress { get; set; }
    public string PaymentEndpoint { get; set; }
    public int RetryCount { get; set; }
    public int RetryWaitTimeInSeconds { get; set; }
}