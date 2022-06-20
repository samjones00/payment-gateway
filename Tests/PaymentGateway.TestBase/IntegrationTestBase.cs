using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Tests.Shared
{
    public class IntegrationTestBase : TestBase
    {
        public HttpClient _httpClient;

        private void SetupLocalhostHttpClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7196")
            };
        }

        private void SetupInMemoryHttpClient()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<IBankConnector>();
                        services.AddTransient<IBankConnector, MockBankConnectorService>(); //replace implementation with a mock
                        //services.AddHttpClient(Constants.ProcessPaymentHttpClientName, client =>
                        //{
                        //    client.BaseAddress = new Uri("http://localhost:5000");
                        //});
                    });
                });

            _httpClient = webApplicationFactory.CreateClient();
        }

        public IntegrationTestBase()
        {
            SetupLocalhostHttpClient();
        }
    }
}