using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Attributes
{
    public class AuthenticationFilter : ActionFilterAttribute,IAsyncActionFilter
    {
        public Role Role{ get; set; }

        public const string ApiKeyHeaderName = "x-api-key";


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authenticationService = (IAuthenticationService)context.HttpContext.RequestServices.GetService(typeof(IAuthenticationService));

            var hasHeader = context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyValue);

            if(!hasHeader || !await authenticationService.IsAuthenticated(apiKeyValue))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await next();
        }
    }
}
