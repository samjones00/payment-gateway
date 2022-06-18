using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Core.Providers;
using PaymentGateway.Core.Services;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Configuration;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.DependencyInjection;

public static class StartupExtensions
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<GatewayOptions>().Bind(configuration.GetSection(GatewayOptions.SectionName));
        services.AddOptions<JwtOptions>().Bind(configuration.GetSection(JwtOptions.SectionName));

        var optionsSection = configuration.GetSection(GatewayOptions.SectionName);
        var options = optionsSection.Get<GatewayOptions>();

        services.AddAutoMapper(typeof(Core.IAssemblyMarker).Assembly);
        services.AddMediatR(typeof(Domain.IAssemblyMarker));
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
        services.AddHttpContextAccessor();

        //Swap out to replace with a different payment provider
        services.AddTransient<IBankConnectorService, BankConnectorService>();

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEncryptionProvider, WeakEncryptionProvider>();

        services.AddHttpClient<BankConnectorService>(Constants.ProcessPaymentHttpClientName, client =>
        {            
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        //TODO:
        //Add polly
        //Validate config   

        return services;
    }

    public static IServiceCollection AddGatewayAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var optionsSection = configuration.GetSection(JwtOptions.SectionName);
        var jwtOptions = optionsSection.Get<JwtOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();
        });

        return services;
    }
}