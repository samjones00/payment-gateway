using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Domain;

namespace PaymentGateway.Tests.Shared
{
    public class IntegrationTestBase : TestBase
    {
        public readonly HttpClient _httpClient;

        public IntegrationTestBase()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // services.RemoveAll(typeof(MockRepository));
                        //services.AddTransient<IRepository, MockRepository>(); //replace implementation with a mock
                        services.AddHttpClient(Constants.PaymentHttpClientName, client =>
                        {
                            client.BaseAddress = new Uri("http://localhost:5000");
                        });
                    });
                });

            _httpClient = application.CreateClient();
        }
    }
}