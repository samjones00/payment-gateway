using System.Net;
using System.Net.Mime;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using PaymentGateway.Domain.Dto;
using PaymentGateway.Tests.Shared;

namespace PaymentGateway.Api.IntegrationTests;

public class PaymentIntegrationTests : IntegrationTestBase
{
    public const string BadRequestPaymentReference = "ba1c9df4-001e-4922-9efa-488b59850bc4";

    [Test]
    public async Task Given_Invalid_CardNumber_When_Processing_Should_Return_Declined()
    {
        // Given
        var paymentRequest = new ProcessPaymentCommand
        {
            PaymentReference = BadRequestPaymentReference
        };

        var content = CreateStringContent(paymentRequest);

        // When
        var response = await _httpClient.PostAsync(Routes.ProcessPayment, content);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Given_Valid_Request_When_Processing_Should_Return_Success()
    {
        // Given
        var paymentRequest = new ProcessPaymentCommand
        {
            PaymentReference = Guid.NewGuid().ToString()
        };

        var content = CreateStringContent(paymentRequest);

        // When
        var response = await _httpClient.PostAsync(Routes.ProcessPayment, content);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    public StringContent CreateStringContent<T>(T model) => new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MediaTypeNames.Application.Json);
}