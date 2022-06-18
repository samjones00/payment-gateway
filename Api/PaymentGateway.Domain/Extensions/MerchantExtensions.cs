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

            var subject = httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (subject is null)
            {
                throw new AuthenticationException($"Claim '{ClaimTypes.NameIdentifier}' cannot be empty.");
            }

            return subject;
        }
    }
}