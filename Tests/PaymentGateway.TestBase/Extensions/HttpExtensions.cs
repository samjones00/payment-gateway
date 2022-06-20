using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace PaymentGateway.Tests.Shared.Extensions
{
    public static class HttpExtensions
    {
        public static StringContent ToStringContent<T>(this T source) where T : new()
            => new(JsonConvert.SerializeObject(source), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}