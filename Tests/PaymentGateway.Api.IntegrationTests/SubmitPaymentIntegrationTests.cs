using System.Net;
using System.Web.Http;
using FluentAssertions;
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
    public async Task Given_Invalid_CardNumber_When_Processing_Should_Return_Declined()
    {
        // Given
        ApplyBearerAuthToken();

        var command = new SubmitPaymentCommand
        {
            PaymentReference = BadRequestPaymentReference
        };

        var content = command.ToStringContent();

        // When
        var response = await _httpClient.PostAsync(ApiRoutes.SubmitPayment, content);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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

    [Test, Ignore("come back to this")]
    public async Task Given_Invalid_Request_When_Processing_Should_Return_Bad_Request()
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

        var validationErrors = GetValidationErrorMessages(result);
    }

    private async Task<IEnumerable<string>> GetValidationErrorMessages(HttpResponseMessage? httpResponseMessage)
    {
        var r = (IHttpActionResult)httpResponseMessage.Content;

        HttpError httpError = await httpResponseMessage.Content.ReadAsAsync<HttpError>();


        //var resultModel = JsonConvert.DeserializeObject<Result>(httpResponseMessage.Content.ReadAsStream(), JsonSerializerHelper.DefaultDeserialisationOptions);


        // KeyValuePair<string,List<string>> er = (KeyValuePair<string, List<string>>)httpError["errors"];
        //var msg = er.Value



        var ab = httpResponseMessage.TryGetContentValue<HttpError>(out var erro);

        var error = httpError.FirstOrDefault(x => x.Key == "errors");
        var json = error.Value;
        // var token = JsonConvert.DeserializeObject<errormessage>(json);
        //var errorMessages = httpError?.Select(x => x.ErrorMessage);

        return default;
    }
}