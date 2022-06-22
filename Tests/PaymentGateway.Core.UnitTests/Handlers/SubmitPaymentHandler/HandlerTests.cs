using AutoMapper;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Responses;
using PaymentGateway.Tests.Shared;
using Handlers = PaymentGateway.Core.Handlers;

namespace SubmitPaymentHandlerTests;

public class HandlerTests : TestBase
{
    private Handlers.SubmitPaymentHandler _sut;

    private Mock<ILogger<Handlers.SubmitPaymentHandler>> _logger;
    private Mock<IBankConnector> _mockBankConnector;
    private Mock<IDateTimeProvider> _mockDateTimeProvider;
    private Mock<IRepository<Payment>> _mockPaymentRepository;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _logger = _mockRepository.Create<ILogger<Handlers.SubmitPaymentHandler>>();
        _mockBankConnector = _mockRepository.Create<IBankConnector>();
        _mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
        _mockPaymentRepository = _mockRepository.Create<IRepository<Payment>>();
        _mockMapper = _mockRepository.Create<IMapper>();

        _mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(new DateTime(2022, 01, 01));
    }

    [Test]
    public async Task Given_Null_Payment_Reference_When_Invoking_Then_Should_Throw_Argument_Null_Exception()
    {
        // Given 
        _sut = new Handlers.SubmitPaymentHandler(_mockBankConnector.Object,
            _mockDateTimeProvider.Object,
            _logger.Object,
            _mockMapper.Object,
            _mockPaymentRepository.Object);

        // When
        var action = () => _sut.Handle(null, It.IsAny<CancellationToken>());

        // Then
        await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("command");
    }

    [Test]
    public async Task Given_Successful_Valid_Payment_Reference_When_Invoking_Then_Should_Return_Response()
    {
        // Given 
        _sut = new Handlers.SubmitPaymentHandler(_mockBankConnector.Object,
            _mockDateTimeProvider.Object,
            _logger.Object,
            _mockMapper.Object,
            _mockPaymentRepository.Object);

        var paymentReference = _fixture.Create<string>();
        var command = Fakes.ValidSubmitPaymentCommand();
        var payment = Fakes.ValidPayment();
        var paymentResponse = _fixture.Create<SubmitPaymentResponse>();

        _mockMapper.Setup(x => x.Map<Payment>(command)).Returns(payment);
        _mockPaymentRepository.Setup(x => x.Insert(payment));
        _mockBankConnector.Setup(x => x.ProcessPayment(payment, It.IsAny<CancellationToken>())).ReturnsAsync(PaymentStatus.Successful);
        _mockPaymentRepository.Setup(x => x.Update(payment)).Returns(payment);
        _mockMapper.Setup(x => x.Map<SubmitPaymentResponse>(payment)).Returns(paymentResponse);

        // When
        var result = await _sut.Handle(command, It.IsAny<CancellationToken>());

        // Then
        result.Should().BeEquivalentTo(paymentResponse);
    }
}