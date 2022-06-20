using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PaymentGateway.Tests.Shared.Extensions
{
    public static class HttpExtensions
    {
        public static StringContent ToStringContent<T>(this T source) where T : new()
            => new(JsonConvert.SerializeObject(source), Encoding.UTF8, MediaTypeNames.Application.Json);

        public static async Task<IEnumerable<string>> GetValidationErrorMessages(this HttpResponseMessage httpResponseMessage)
        {
            var validationProblemDetails = await httpResponseMessage.Content.ReadAsAsync<ValidationProblemDetails>();
            return validationProblemDetails.Errors.Select(x => x.Value).SelectMany(x => x);         
        }
    }
}