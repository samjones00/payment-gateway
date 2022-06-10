using System.Net.Http;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Services;
public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IHttpClientFactory httpClientFactory, ILogger<PaymentService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClientFactory.CreateClient(Constants.PaymentHttpClientName);
    }

    public async Task Process()
    {
        var response = await _httpClient.GetAsync("wee");

        var payment = new Payment();
    }

}
