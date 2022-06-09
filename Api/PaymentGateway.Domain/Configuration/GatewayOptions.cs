namespace PaymentGateway.Domain.Configuration;
public class GatewayOptions
{
    public const string SectionName = nameof(GatewayOptions);

    public string BaseAddress { get; set; }
}
