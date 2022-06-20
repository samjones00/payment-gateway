using System.Net;
using System.Web.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Constants;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Enums;
using PaymentGateway.Tests.Shared.Extensions;

namespace PaymentGateway.Api.IntegrationTests;

public class SubmitPaymentIntegrationTests : IntegrationTestBase
{
    public const string BadRequestPaymentReference = "ba1c9df4-001e-4922-9efa-488b59850bc4";

    [SetUp]
    public void SetUp()
    {
        SetupHttpClient(HttpClientType.InMemory);
    }
    
    [Test]
    public async Task Given_Valid_Request_When_Processing_Should_Return_Created()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Given_Declined_CardNumber_When_Processing_Should_Return_Declined()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.PaymentReference = BadRequestPaymentReference;

        var content = command.ToStringContent();

        // When
        var response = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Given_Invalid_Amount_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.Amount = -12;

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain("'Amount' must be greater than '0.0'.");
    }

    [Test]
    public async Task Given_Invalid_CardHolder_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.CardHolder = "123";

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain("'Card Holder' is not in the correct format.");
    }

    [Test]
    public async Task Given_Invalid_CardNumber_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.CardNumber = "ABC";

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain("'Card Number' is not in the correct format.");
    }

    [Test]
    public async Task Given_Invalid_Currency_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.Currency = "123";

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain("'Currency' is not in the correct format.");
    }

    [Test]
    public async Task Given_Invalid_CVV_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.CVV = "ABC";

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain("'CVV' is not in the correct format.");
    }

    [Test]
    public async Task Given_Invalid_ExpiryDateMonth_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.ExpiryDateMonth = 23;

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain("'Expiry Date Month' must be between 1 and 12. You entered 23.");
    }

    [Test]
    public async Task Given_Invalid_ExpiryDateYear_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.ExpiryDateYear = DateTime.Now.AddYears(-1).Year;

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain($"'Expiry Date Year' must be greater than or equal to '{DateTime.Now.Year}'.");
    }

    [Test]
    public async Task Given_Invalid_PaymentReference_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.PaymentReference = "...";

        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationErrors = await result.GetValidationErrorMessages();

        validationErrors.Should().Contain("'Payment Reference' must be 36 characters in length. You entered 3 characters.");
    }
}