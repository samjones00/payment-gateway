using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.DependencyInjection
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(ApiRoutes.SubmitPayment, async ([FromServices] IMediator mediator, SubmitPaymentCommand command) =>
            {
                var response = await mediator.Send(command);

                if (response.PaymentStatus is PaymentStatus.Unsuccessful)
                {
                    return Results.BadRequest(response);
                }

                return Results.Created($"{ApiRoutes.SubmitPayment}/{response.PaymentReference}", response);
            })
            .Produces<SubmitPaymentResponse>(StatusCodes.Status400BadRequest)
            .Produces<SubmitPaymentResponse>(StatusCodes.Status201Created);

            endpoints.MapPost(ApiRoutes.GetPaymentDetails, async ([FromServices] IMediator mediator, PaymentDetailsQuery query) =>
            {
                return await mediator.Send(query) is PaymentDetailsResponse details ? Results.Ok(details) : Results.NotFound();
            })
            .Produces<PaymentDetailsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}