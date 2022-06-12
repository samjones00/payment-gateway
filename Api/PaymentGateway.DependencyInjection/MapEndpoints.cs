using MediatR;
using Microsoft.AspNetCore.Http;
using PaymentGateway.Core.Responses;
using PaymentGateway.Domain.Dto;
using PaymentGateway.Domain.Queries;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;


namespace PaymentGateway.DependencyInjection
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/payment/process", async ([FromServices] IMediator mediator, ProcessPaymentCommand command) =>
             await mediator.Send(command) is ProcessPaymentResponse payment ? Results.Ok(payment) : Results.NotFound())
                .Produces<ProcessPaymentResponse>(StatusCodes.Status201Created);

            endpoints.MapPost("/payment/details", async ([FromServices] IMediator mediator, PaymentDetailsQuery query) =>
                     await mediator.Send(query) is PaymentDetailsResponse details ? Results.Ok(details) : Results.NotFound())
               .Produces<PaymentDetailsResponse>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}