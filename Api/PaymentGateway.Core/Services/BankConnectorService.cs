using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Services
{
    public class BankConnectorService : IBankConnectorService
    {
        private readonly HttpClient _httpClient;

        public BankConnectorService(IHttpClientFactory httpClientFactory)
        {
            ArgumentNullException.ThrowIfNull(httpClientFactory);

            _httpClient = httpClientFactory.CreateClient(Constants.ProcessPaymentHttpClientName);
        }

        public async Task<Payment> Process(Payment payment, CancellationToken cancellationToken)
        {
            var content = CreateStringContent(payment);
            var response = await _httpClient.PostAsync($"/process/{payment.PaymentReference.Value}", content, cancellationToken);

            payment.PaymentStatus = response.IsSuccessStatusCode ? PaymentStatus.Successful : PaymentStatus.Unsuccessful;

            return payment;
        }

        public static StringContent CreateStringContent<T>(T model) => new(JsonConvert.SerializeObject(model), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}