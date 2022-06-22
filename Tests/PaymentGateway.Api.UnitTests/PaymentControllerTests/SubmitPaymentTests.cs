using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Responses;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Extensions;

namespace PaymentControllerTests;

public class SubmitPaymentTests : TestBase
{
    private PaymentController _sut;

    private Mock<ILogger<PaymentController>> _logger;
    private Mock<IMediator> _mockMediator;
    private Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    [SetUp]
    public void Setup()
    {
        _logger = _mockRepository.Create<ILogger<PaymentController>>();
        _mockMediator = _mockRepository.Create<IMediator>();
        _mockHttpContextAccessor = _mockRepository.Create<IHttpContextAccessor>();

        _sut = new PaymentController(_logger.Object, _mockMediator.Object, _mockHttpContextAccessor.Object);
    }

    [Test]
    public async Task Given_Null_Payment_Reference_When_Invoking_Then_Sholuld_Throw_Argument_Null_Exception()
    {
        // Given & When
        var response = await _sut.SubmitPayment(null);

        // Then
        var httpResult = response as BadRequestObjectResult;
        httpResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        httpResult.Value.Should().Be("Value cannot be null. (Parameter 'command')");
    }

    [Test]
    public async Task Given_Null_Claim_When_Invoking_Then_Should_Throw_Authentication_Exception()
    {
        // Given
        _mockHttpContextAccessor.SetupWithoutClaim();

        var command = Fakes.ValidSubmitPaymentCommand();

        // When
        var response = await _sut.SubmitPayment(command);

        // Then
        var httpResult = response as UnauthorizedObjectResult;
        httpResult.Value.Should().Be("Claim 'NameIdentifier' not found.");
    }

    [Test]
    public async Task Given_Successful_Command_When_Invoking_Then_Should_Return_Created_At_Action_Result()
    {
        // Given
        _mockHttpContextAccessor.SetupWithClaim();

        var command = Fakes.ValidSubmitPaymentCommand();
        var response = _fixture.Build<SubmitPaymentResponse>()
            .With(x => x.PaymentStatus, PaymentStatus.Successful.ToString())
            .Create();

        _mockMediator.Setup(m => m.Send(It.IsAny<SubmitPaymentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // When
        var result = await _sut.SubmitPayment(command);

        // Then
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task Given_Unsuccessful_Command_When_Invoking_Then_Should_Return_Bad_Request_Object_Result()
    {
        // Given
        _mockHttpContextAccessor.SetupWithClaim();

        var command = Fakes.ValidSubmitPaymentCommand();
        var response = _fixture.Build<SubmitPaymentResponse>()
            .With(x => x.PaymentStatus, PaymentStatus.Unsuccessful.ToString())
            .Create();

        _mockMediator.Setup(m => m.Send(It.IsAny<SubmitPaymentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // When
        var result = await _sut.SubmitPayment(command);

        // Then
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}