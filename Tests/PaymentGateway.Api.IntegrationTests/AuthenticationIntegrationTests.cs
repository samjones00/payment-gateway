using System.Net;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Domain.Constants;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Tests.Shared.Enums;
using PaymentGateway.Tests.Shared.Extensions;

namespace PaymentGateway.Api.IntegrationTests;

public class AuthenticationIntegrationTests : IntegrationTestBase
{
    [SetUp]
    public void SetUp()
    {
        SetupHttpClient(HttpClientType.InMemory);
    }

    [Test]
    public async Task Given_Invalid_Bearer_Token_When_Sending_Valid_Command_Should_Return_Forbidden()
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
    public async Task Given_Invalid_Bearer_Token_When_Requesting_Payment_Details_Should_Return_Forbidden()
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