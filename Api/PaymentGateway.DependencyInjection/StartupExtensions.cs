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
    public static IServiceCollection AddPaymentConfiguration(this IServiceCollection services, IConfiguration configuration, Action<GatewayOptions>? setupAction = null)
    {
        var optionsSection = configuration.GetSection(GatewayOptions.SectionName);

        services.AddOptions<GatewayOptions>()
                   .Bind(optionsSection)
                   .Configure(setupAction);

        return services;
    }

    public static IServiceCollection AddPaymentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<GatewayOptionsValidator>();

        return services;
    }

    public static IServiceCollection AddPaymentProcessingServices(this IServiceCollection services, IConfiguration configuration)
    {
        var optionsSection = configuration.GetSection(GatewayOptions.SectionName);
        var options = optionsSection.Get<GatewayOptions>();

        //services.AddTransient<IProcessPaymentService, ProcessPaymentHandler>();

        //Swap out to replace with a different payment provider
        services.AddTransient<IBankConnectorService, BankConnectorService>();

        services.AddHttpClient<BankConnectorService>(Constants.PaymentHttpClientName, client =>
        {
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        return services;
    }

    public static IServiceCollection AddPaymentDetailServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<GatewayOptionsValidator>();
        //services.AddTransient<IPaymentDetailsService, PaymentDetailsService>();

        return services;
    }

    //TODO:
    //Add polly
    //Validate config   
}