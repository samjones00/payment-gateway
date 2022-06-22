using System.Security.Authentication;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Extensions;

namespace PaymentControllerTests;

public class GetPaymentDetailsTests : TestBase
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

    [TestCaseSource(nameof(NullOrWhiteSpaceStrings))]
    public async Task Given_Null_Payment_Reference_When_Invoking_Then_Should_Throw_Argument_Null_Exception(string paymentReference)
    {
        // Given & When
        var result = await _sut.GetPaymentDetails(paymentReference);

        // Then
        var httpResult = result as BadRequestObjectResult;
        httpResult.Value.Should().Be("Value cannot be null. (Parameter 'paymentReference')");
    }

    [Test]
    public void Given_Null_Claim_When_Invoking_Then_Should_Throw_Authentication_Exception()
    {
        // Given
        var paymentReference = _fixture.Create<string>();
        var command = Fakes.ValidSubmitPaymentCommand();

        // When
        var result = () => _sut.GetPaymentDetails(paymentReference);

        // Then
        result.Should().ThrowAsync<AuthenticationException>().WithMessage($"Claim '{ClaimTypes.NameIdentifier}' cannot be empty.");
    }

    [Test]
    public async Task Given_Existing_Payment_Reference_When_Invoking_Then_Should_Return_Ok_Object_Result()
    {
        // Given
        _mockHttpContextAccessor.SetupWithClaim();

        var paymentReference = _fixture.Create<string>();
        var response = _fixture.Build<PaymentDetailsResponse>()
            .With(x => x.PaymentStatus, PaymentStatus.Successful.ToString())
            .Create();

        _mockMediator.Setup(m => m.Send(It.IsAny<PaymentDetailsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // When
        var result = await _sut.GetPaymentDetails(paymentReference);

        // Then
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task Given_Non_Existing_Payment_Reference_When_Invoking_Then_Should_Return_Not_Found()
    {
        // Given
        _mockHttpContextAccessor.SetupWithClaim();

        var merchantReference = _fixture.Create<string>();
        var paymentReference = _fixture.Create<string>();
        var response = _fixture.Build<PaymentDetailsResponse>()
            .With(x => x.PaymentStatus, PaymentStatus.Unsuccessful.ToString())
            .Create();

        _mockMediator.Setup(m => m.Send(It.IsAny<PaymentDetailsQuery>(), It.IsAny<CancellationToken>()))
            .Throws(new PaymentNotFoundException($"Payment not found", paymentReference, merchantReference));

        // When
        var result = await _sut.GetPaymentDetails(paymentReference);

        // Then
        var httpResult = result as NotFoundObjectResult;
        httpResult.Value.Should().Be($"Payment not found. payment reference: {paymentReference}, merchant reference: {merchantReference}");
    }
}