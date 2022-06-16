using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Api.IntegrationTests;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Tests.Shared
{
    public class IntegrationTestBase : TestBase
    {
        private WebApplicationFactory<Program> _webApplicationFactory;

        public HttpClient _httpClient;

        public void Build() => _httpClient = _webApplicationFactory.CreateClient();

        public IntegrationTestBase()
        {
            _webApplicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<IBankConnectorService>();
                        services.AddTransient<IBankConnectorService, MockBankConnectorService>(); //replace implementation with a mock
                        //services.AddHttpClient(Constants.ProcessPaymentHttpClientName, client =>
                        //{
                        //    client.BaseAddress = new Uri("http://localhost:5000");
                        //});
                    });
                });


            _httpClient = _webApplicationFactory.CreateClient();
        }

        public void ReplaceService<TInterface, TClass>()
        {
            _webApplicationFactory = new WebApplicationFactory<Program>()
                 .WithWebHostBuilder(builder =>
                 {
                     builder.ConfigureServices(services =>
                     {
                         services.RemoveAll(typeof(TInterface));
                         services.AddTransient(typeof(TInterface), typeof(TClass));
                     });
                 });
        }
    }
}