using Microsoft.AspNetCore.Builder;
using PaymentGateway.Core.Middleware;

namespace PaymentGateway.DependencyInjection
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
