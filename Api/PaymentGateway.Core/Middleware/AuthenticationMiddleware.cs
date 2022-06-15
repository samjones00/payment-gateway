using System.Net;
using Microsoft.AspNetCore.Http;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Middleware
{
    public class AuthenticationMiddleware : IMiddleware
    {
        public const string ApiKeyHeaderName = "x-api-key";

        private readonly IAuthenticationService _authenticationService;

        public AuthenticationMiddleware(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var hasHeader = context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyValue);

            if (!hasHeader || !await _authenticationService.IsAuthenticated(apiKeyValue))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await next(context);
        }
    }
}