namespace PaymentGateway.Domain.Configuration;
public class JwtOptions
{
    public const string SectionName = nameof(JwtOptions);

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;
}