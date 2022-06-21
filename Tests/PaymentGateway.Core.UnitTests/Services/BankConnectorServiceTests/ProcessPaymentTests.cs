using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq.Protected;
using PaymentGateway.AcquiringBank.CKO.Models;
using PaymentGateway.Core.Services;
using PaymentGateway.Domain.Configuration;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Extensions;

namespace BankConnectorServiceTests;

public class ProcessPaymentTests : TestBase
{
    private BankConnectorService<Request, Response> _sut;

    private IOptions<AcquiringBankOptions> _options;
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private Mock<ILogger<BankConnectorService<Request, Response>>> _logger;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _options = Options.Create(new AcquiringBankOptions
        {
            BaseAddress = "http://localhost:1234",
            PaymentEndpoint = "/payment/PaymentReference",
            RetryCount = 5,
            RetryWaitTimeInSeconds = 2
        });

        _logger = _mockRepository.Create<ILogger<BankConnectorService<Request, Response>>>();
        _mockHttpClientFactory = _mockRepository.Create<IHttpClientFactory>();
        _mockMapper = _mockRepository.Create<IMapper>();
    }

    [Test]
    public async Task Given_Null_Payment_When_Invoking_Should_Throw_Argument_Null_Exception()
    {
        // Given
        _mockHttpClientFactory.SetupCreateClient(new HttpClient());

        // When
        _sut = new BankConnectorService<Request, Response>(_logger.Object, _mockHttpClientFactory.Object, _mockMapper.Object, _options);

        var action = async () => await _sut.ProcessPayment(null, new CancellationToken());
        // Then

        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("payment");
    }

    [Test]
    public async Task Given_Unsuccessful_Payment_When_Invoking_Should_Return_Bad_Request()
    {
        // Given
        var payment = Fakes.ValidPayment();
        var handlerMock = CreateMockHttpMessageHandler(HttpStatusCode.BadRequest);

        _mockHttpClientFactory.SetupCreateClient(new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(_options.Value.BaseAddress),
        });

        _mockMapper.Setup(x => x.Map<Request>(It.IsAny<Payment>())).Returns(_fixture.Create<Request>());
        _logger.SetupMockLogger();

        _sut = new BankConnectorService<Request, Response>(_logger.Object, _mockHttpClientFactory.Object, _mockMapper.Object, _options);

        // When 
        var result = await _sut.ProcessPayment(payment, new CancellationToken());

        // Then
        result.Should().Be(PaymentStatus.Unsuccessful);
    }

    [Test]
    public async Task Given_Successful_Payment_When_Invoking_Should_Return_Created()
    {
        // Given
        var payment = Fakes.ValidPayment();
        var handlerMock = CreateMockHttpMessageHandler(HttpStatusCode.Accepted);

        _mockHttpClientFactory.SetupCreateClient(new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(_options.Value.BaseAddress),
        });

        _mockMapper.Setup(x => x.Map<Request>(It.IsAny<Payment>())).Returns(_fixture.Create<Request>());
        _logger.SetupMockLogger();

        _sut = new BankConnectorService<Request, Response>(_logger.Object, _mockHttpClientFactory.Object, _mockMapper.Object, _options);

        // When 
        var result = await _sut.ProcessPayment(payment, new CancellationToken());

        // Then
        result.Should().Be(PaymentStatus.Successful);
    }

    private Mock<HttpMessageHandler> CreateMockHttpMessageHandler(HttpStatusCode statusCode, HttpContent content = null)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = statusCode,
               Content = content
           })
           .Verifiable();

        return handlerMock;
    }
}