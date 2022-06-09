using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Core.Services;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Configuration;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Validators;

namespace PaymentGateway.DependencyInjection;

public static class StartupExtensions
{
    public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration, Action<GatewayOptions>? setupAction = null)
    {
        var optionsSection = configuration.GetSection(GatewayOptions.SectionName);
        var options = optionsSection.Get<GatewayOptions>();

        services.AddOptions<GatewayOptions>()
            .Bind(optionsSection)
            .Configure(setupAction);

        services.AddTransient<IPaymentService, PaymentService>();
        services.AddValidatorsFromAssemblyContaining<GatewayOptionsValidator>();

        services.AddHttpClient(Constants.PaymentHttpClientName, client =>
        {
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        return services;
    }

    //TODO: Add polly
}