using System.Net;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Domain.Constants;
using PaymentGateway.Domain.Responses;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Extensions;

namespace IntegrationTests;

public class SubmitPaymentIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Given_Valid_Request_When_Processing_Should_Return_Created()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.PaymentReference = _fixture.Create<string>();
        var content = command.ToStringContent();

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Given_Unsuccessful_CardNumber_When_Processing_Should_Return_Unsuccessful()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.PaymentReference = Constants.UnsuccessfulPaymentReference;
        var content = command.ToStringContent();

        // When
        var response = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var expectedResponse = new SubmitPaymentResponse
        {
            PaymentReference = command.PaymentReference,
            PaymentStatus = "Unsuccessful"
        };

        var actualResponse = await response.Content.ReadAsAsync<SubmitPaymentResponse>();

        expectedResponse.Should().BeEquivalentTo(actualResponse, assertionOptions => assertionOptions.Excluding(x => x.ProcessedOn));
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

        validationErrors.Count().Should().Be(1);
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

        validationErrors.Count().Should().Be(1);
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

        validationErrors.Count().Should().Be(2);

        validationErrors.Should().Contain("'Card Number' is not in the correct format.");
        validationErrors.Should().Contain($"'Card Number' must be between 14 and 16 characters. You entered {command.CardNumber.Length} characters.");
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

        validationErrors.Count().Should().Be(1);
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

        validationErrors.Count().Should().Be(1);
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

        validationErrors.Count().Should().Be(1);
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

        validationErrors.Count().Should().Be(1);
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

        validationErrors.Count().Should().Be(1);
        validationErrors.Should().Contain("'Payment Reference' must be 36 characters in length. You entered 3 characters.");
    }
}