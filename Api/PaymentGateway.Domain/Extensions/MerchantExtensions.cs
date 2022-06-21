using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PaymentGateway.Domain.Extensions
{
    public static class MerchantExtensions
    {
        public static string GetMerchantReference(this IHttpContextAccessor httpContextAccessor)
        {
            ArgumentNullException.ThrowIfNull(httpContextAccessor);

            static bool typeExpression(Claim x) => x.Type == ClaimTypes.NameIdentifier;

            var exists = httpContextAccessor.HttpContext.User.Claims.Any(typeExpression);

            if (!exists)
            {
                throw new AuthenticationException($"Claim '{nameof(ClaimTypes.NameIdentifier)}' not found.");
            }

            return httpContextAccessor.HttpContext.User.Claims.Single(typeExpression)?.Value;
        }
    }
}