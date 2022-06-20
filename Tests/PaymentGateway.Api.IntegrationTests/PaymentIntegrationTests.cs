using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;
using PaymentGateway.Tests.Shared;

namespace PaymentGateway.Api.IntegrationTests;

public class PaymentIntegrationTests : IntegrationTestBase
{
    public const string BadRequestPaymentReference = "ba1c9df4-001e-4922-9efa-488b59850bc4";

    [Test]
    public async Task Given_Invalid_CardNumber_When_Processing_Should_Return_Declined()
    {
        // Given
        var paymentRequest = new SubmitPaymentCommand
        {
            PaymentReference = BadRequestPaymentReference
        };

        var content = CreateStringContent(paymentRequest);

        // When
        var response = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Test]
    public async Task Given_Valid_Request_When_Processing_Should_Return_Created()
    {
        // Given
        ApplyAuthenticationHeader();

        var paymentRequest = new SubmitPaymentCommand
        {
            PaymentReference = CreateStringOfLength(PaymentReference.Length),
            Amount = 12.34m,
            CardNumber = CreateStringOfLength(CardNumber.MinimumLength),
            CVV = CreateStringOfLength(CVV.MinimumLength),
            MerchantReference = CreateStringOfLength(20),
            Currency = "GBP",
            CardHolder = "Sam Jones",
            ExpiryDateMonth = 12,
            ExpiryDateYear = 2022
        };

        var content = CreateStringContent(paymentRequest);

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Given_Valid_Request_When_Processing_Should_Return_Bad_Request()
    {
        // Given
        var paymentRequest = new SubmitPaymentCommand
        {
            Amount = 12.34m,
            CardNumber = CreateStringOfLength(CardNumber.MinimumLength),
            CVV = CreateStringOfLength(CVV.MinimumLength),
            MerchantReference = CreateStringOfLength(20),
            Currency = "GBP",
            CardHolder = "Sam Jones",
            ExpiryDateMonth = 12,
            ExpiryDateYear = 2021
        };

        var content = CreateStringContent(paymentRequest);

        // When
        var result = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await result.Content.ReadAsStringAsync();

        json.Should().Be("'Year' must be greater than or equal to '2022'.");

    }

    private void ApplyAuthenticationHeader()
    {
        var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsImlhdCI6MTY1NTU4MjkxMCwiZXhwIjoxNjg3MTE4OTEwLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsInN1YiI6IkFwcGxlIn0.TLGIHiqFuAbM7cVIJ3ZKVQ3dLi9YSzLE2BYVRqKqPhk";
        var authHeader = new AuthenticationHeaderValue("Bearer", token);

        _httpClient.DefaultRequestHeaders.Authorization = authHeader;
    }

    public StringContent CreateStringContent<T>(T model) => new(JsonConvert.SerializeObject(model), Encoding.UTF8, MediaTypeNames.Application.Json);
}