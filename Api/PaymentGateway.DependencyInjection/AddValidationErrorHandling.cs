using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace PaymentGateway.DependencyInjection
{
    public static class ValidationErrorHandlingMiddleware
    {
        public static IApplicationBuilder AddValidationErrorHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error.InnerException is ValidationException)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.InnerException.Message);
                    }
                });
            });

            return app;
        }
    }
}