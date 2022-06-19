using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.DependencyInjection
{
    public static class ExceptionHandlingMiddleware
    {
        public static IApplicationBuilder AddExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    if (exception is PaymentNotFoundException)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync(exception.Message);
                    }

                    if (exception is PaymentAlreadyExistsException)
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsync(exception.Message);
                    }

                    if (exception is AcquiringBankException)
                    {
                        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        await context.Response.WriteAsync(exception.Message);
                    }
                });
            });

            return app;
        }
    }
}