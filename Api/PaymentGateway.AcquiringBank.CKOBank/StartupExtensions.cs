using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.AcquiringBank.CKO.Models;
using PaymentGateway.Core.Services;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Configuration;
using PaymentGateway.Domain.Interfaces;
using Polly;
using Polly.Extensions.Http;

namespace PaymentGateway.AcquiringBank.CKO
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCKOBankServices(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(AcquiringBankOptions.SectionName);
            var options = section.Get<AcquiringBankOptions>();

            services
                .AddHttpClient<BankConnectorService<Request, Response>>(HttpClientNames.ProcessPaymentHttpClientName, client =>
                {
                    client.BaseAddress = new Uri(options.BaseAddress);
                    client.ConfigureBankEndpointAuth();
                })
                .AddPolicyHandler(GetRetryPolicy(options));

            services.AddOptions<AcquiringBankOptions>().Bind(section);
            services.AddTransient<IBankConnector, BankConnectorService<Request, Response>>();
            services.AddAutoMapper(typeof(IAssemblyMarker).Assembly);

            return services;
        }

        public static ConfigureHostBuilder AddCKOBankConfiguration(this ConfigureHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                var assembly = Assembly.GetExecutingAssembly();

                config
                    .SetBasePath(Path.GetDirectoryName(assembly.Location))
                    .AddJsonFile($"{assembly.GetName().Name}.appsettings.json");
            });

            return builder;
        }

        private static void ConfigureBankEndpointAuth(this HttpClient httpClient)
        {
            // Configure auth for bank endpoint here
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(AcquiringBankOptions options)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(options.RetryCount,
                    i => TimeSpan.FromSeconds(options.RetryWaitTimeInSeconds),
                    (result, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine(
                                $"Request failed with {result?.Result?.StatusCode}. Waiting {timeSpan} before next retry. " +
                                $"Retry attempt {retryCount} of {options.RetryCount}");
                    });
        }
    }
}