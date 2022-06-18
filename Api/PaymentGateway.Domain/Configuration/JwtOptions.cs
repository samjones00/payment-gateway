namespace PaymentGateway.Domain.Configuration;
public class JwtOptions
{
    public const string SectionName = nameof(JwtOptions);

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Key { get; set; }
}