﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.AcquiringBanks.CKO.Models;
using PaymentGateway.Core.Services;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Configuration;
using PaymentGateway.Domain.Interfaces;
using Polly;
using Polly.Extensions.Http;

namespace PaymentGateway.AcquiringBanks.CKO
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCKOBankServices(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("CKOBankOptions");
            var options = section.Get<AcquiringBankOptions>();

            services
                .AddHttpClient<BankConnectorService<Request, Response>>(HttpClientNames.ProcessPaymentHttpClientName, client =>
                {
                    client.BaseAddress = new Uri(options.BaseAddress);
                    client.DefaultRequestHeaders.Add("x-secure-key", "madeup-secure-key");
                })
                .AddPolicyHandler(GetRetryPolicy(options));

            services.AddOptions<AcquiringBankOptions>().Bind(section);
            services.AddTransient<IBankConnector, BankConnectorService<Request, Response>>();
            services.AddAutoMapper(typeof(IAssemblyMarker).Assembly);

            return services;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(AcquiringBankOptions options)
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
            //.WaitAndRetryAsync(options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        //public static void AddPolicies(this HttpClient client)
        //{
        //    client.AddPolicyHandler(HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .WaitAndRetryAsync(httpClientOptions.RetryPolicy.RetryCount,
        //            i => TimeSpan.FromSeconds(httpClientOptions.RetryPolicy.RetryWaitTimeInSeconds),
        //            (result, timeSpan, retryCount, context) =>
        //            {
        //                Console.WriteLine(
        //                        $"Request failed with {result?.Result?.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount} OF {httpClientOptions.RetryPolicy.RetryCount}");
        //            }));

        //    if (httpClientOptions.CircuitBreakerPolicy.Enabled)
        //        client.AddPolicyHandler(HttpPolicyExtensions
        //            .HandleTransientHttpError()
        //            .CircuitBreakerAsync(httpClientOptions.CircuitBreakerPolicy.EventsBeforeBreak,
        //                TimeSpan.FromSeconds(httpClientOptions.CircuitBreakerPolicy.DurationOfBreakInSeconds)));
        //}
    }
}