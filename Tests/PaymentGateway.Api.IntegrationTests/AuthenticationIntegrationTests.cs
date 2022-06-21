using System.Net;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Domain.Constants;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Extensions;

namespace IntegrationTests;

public class AuthenticationIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Given_No_Bearer_Token_When_Sending_Valid_Command_Should_Return_Unauthorized()
    {
        // Given
        var command = Fakes.ValidSubmitPaymentCommand();
        var content = command.ToStringContent();

        // When
        var response = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Given_No_Bearer_Token_When_Requesting_Payment_Details_Should_Return_Unauthorized()
    {
        // Given
        var paymentReference = _fixture.Create<string>();
        var requestUri = ApiRoutes.GetPaymentDetails.Replace(HttpClientConstants.PaymentReference, paymentReference);

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}