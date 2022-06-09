using System.Net.Http;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Services;
public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IHttpClientFactory httpClientFactory, ILogger<PaymentService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClientFactory.CreateClient(Constants.PaymentHttpClientName);
        _logger = logger;
    }

    public async Task Process()
    {
        var response = await _httpClient.GetAsync("wee");
    }

}
