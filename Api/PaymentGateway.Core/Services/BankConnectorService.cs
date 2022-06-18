using System.Net;
using System.Net.Mime;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Configuration;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Services
{
    public class BankConnectorService<TBankRequest, TBankResponse> : IBankConnector
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IBankConnector> _logger;
        private readonly IMapper _mapper;
        private readonly AcquiringBankOptions _options;

        public BankConnectorService(ILogger<IBankConnector> logger, IHttpClientFactory httpClientFactory, IMapper mapper, IOptions<AcquiringBankOptions> options)
        {
            ArgumentNullException.ThrowIfNull(httpClientFactory);
            ArgumentNullException.ThrowIfNull(options);

            _httpClient = httpClientFactory.CreateClient(HttpClientNames.ProcessPaymentHttpClientName);
            _logger = logger ?? throw new NullReferenceException(nameof(logger));
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
            _options = options.Value;
        }

        public async Task<PaymentStatus> Process(Payment payment, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<TBankRequest>(payment);
            var content = CreateStringContent(request);
            var url = _options.PaymentEndpoint.Replace(nameof(payment.PaymentReference), payment.PaymentReference.Value);

            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation(json);

            return HandleStatusCode(response.StatusCode);
        }

        public static PaymentStatus HandleStatusCode(HttpStatusCode statusCode) => statusCode switch
        {
            HttpStatusCode.OK => PaymentStatus.Successful,
            HttpStatusCode.BadRequest => PaymentStatus.Unsuccessful,
            HttpStatusCode.Accepted => PaymentStatus.Pending,
            HttpStatusCode.Conflict => PaymentStatus.Unsuccessful,
            _ => throw new AcquiringBankException($"Unexpected status code: {statusCode}"),
        };

        public static StringContent CreateStringContent<T>(T model) => new(JsonConvert.SerializeObject(model), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}