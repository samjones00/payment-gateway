using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Tests.Shared.Enums;
using PaymentGateway.Tests.Shared.Mocks;

namespace PaymentGateway.Tests.Shared
{
    public class IntegrationTestBase : TestBase
    {
        public HttpClient _httpClient;

        public void SetupHttpClient(HttpClientType httpClientType)
        {
            _httpClient = httpClientType == HttpClientType.InMemory ? SetupInMemoryHttpClient() : SetupLocalhostHttpClient();
        }

        public void ApplyBearerAuthToken()
        {
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2" +
                "xvY2FsaG9zdDo1MTM4LyIsImlhdCI6MTY1NTU4MjkxMCwiZXhwIjoxNjg3MTE4OTEwL" +
                "CJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsInN1YiI6IkFwcGxlIn0.TLGI" +
                "HiqFuAbM7cVIJ3ZKVQ3dLi9YSzLE2BYVRqKqPhk";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private HttpClient SetupLocalhostHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7196")
            };
        }

        private HttpClient SetupInMemoryHttpClient()
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