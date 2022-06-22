using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Tests.Shared.Mocks;

namespace PaymentGateway.Tests.Shared
{
    public class IntegrationTestBase : TestBase
    {
        public HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            SetupHttpClient();
        }

        public void ApplyBearerAuthToken()
        {
            var token = TestContext.Parameters["BearerToken"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private void SetupHttpClient()
        {
            var useInMemoryHttpClient = TestContext.Parameters["UseInMemoryHttpClient"].Equals("true", StringComparison.InvariantCultureIgnoreCase);

            _httpClient = useInMemoryHttpClient ? SetupInMemoryHttpClient() : SetupLocalhostHttpClient();
        }

        private static HttpClient SetupLocalhostHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri(TestContext.Parameters["GatewayBaseUrl"])
            };
        }

        private static HttpClient SetupInMemoryHttpClient()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<IBankConnector>();
                        services.AddTransient<IBankConnector, MockBankConnectorService>(); //replace implementation with a mock
                    });
                });

            return webApplicationFactory.CreateClient();
        }
    }
}