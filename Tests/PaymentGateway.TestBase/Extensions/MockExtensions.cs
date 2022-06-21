using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PaymentGateway.AcquiringBank.CKO.Models;
using PaymentGateway.Core.Services;

namespace PaymentGateway.Tests.Shared.Extensions
{
    public static class MockExtensions
    {
        public static void SetupWithClaim(this Mock<IHttpContextAccessor> mockHttpContextAccessor)
        {
            mockHttpContextAccessor.Setup(x => x.HttpContext.User.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "MerchantReferenceHere")
            });
        }

        public static void SetupWithoutClaim(this Mock<IHttpContextAccessor> mockHttpContextAccessor)
        {
            mockHttpContextAccessor.Setup(x => x.HttpContext.User.Claims).Returns(new List<Claim>());
        }

        public static void SetupCreateClient(this Mock<IHttpClientFactory> mockHttpClientFactory, HttpClient httpClient)
        {
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        }

        public static void SetupMockLogger(this Mock<ILogger<BankConnectorService<Request, Response>>> mockLogger)
        {
            mockLogger.Setup(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }
    }
}