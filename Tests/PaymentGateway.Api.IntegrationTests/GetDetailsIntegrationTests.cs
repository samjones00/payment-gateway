using System.Net;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Domain.Constants;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Extensions;
using PaymentGateway.Domain.Responses;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Extensions;

namespace IntegrationTests;

public class GetDetailsIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Given_Invalid_PaymentReference_When_Requesting_Payment_Details_Should_Return_NotFound()
    {
        // Given
        ApplyBearerAuthToken();

        var paymentReference = _fixture.Create<string>();
        var requestUri = ApiRoutes.GetPaymentDetails.Replace(HttpClientConstants.PaymentReference, paymentReference);

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Given_Valid_PaymentReference_When_Requesting_Payment_Details_Should_Return_PaymentDetailsResponse()
    {
        // Given
        ApplyBearerAuthToken();

        var command = Fakes.ValidSubmitPaymentCommand();
        command.PaymentReference = _fixture.Create<string>();
        var content = command.ToStringContent();

        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);
        var url = result.Headers.Location;

        // When
        var response = await _httpClient.GetAsync(url);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var expectedResponse = new PaymentDetailsResponse
        {
            Amount = command.Amount,
            CardNumber = command.CardNumber.ToMaskedString(),
            IsAuthorised = true,
            PaymentReference = command.PaymentReference,
            PaymentStatus = PaymentStatus.Successful.ToString(),
            Currency = command.Currency,
        };

        var actualResponse = await response.Content.ReadAsAsync<PaymentDetailsResponse>();

        expectedResponse.Should().BeEquivalentTo(actualResponse, assertionOptions => assertionOptions.Excluding(x => x.ProcessedOn));
    }
}