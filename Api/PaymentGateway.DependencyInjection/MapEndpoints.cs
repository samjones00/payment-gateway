using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Extensions;
using PaymentGateway.Domain.Queries;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.DependencyInjection
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapPost(ApiRoutes.SubmitPayment, async (
                    [FromServices] IMediator mediator,
                    [FromServices] IHttpContextAccessor httpContextAccessor,
                    [FromBody] SubmitPaymentDto command,
                    CancellationToken cancellationToken) => await Submit(mediator, httpContextAccessor, command, cancellationToken))
                .Produces<SubmitPaymentResponse>(StatusCodes.Status202Accepted)
                .Produces<SubmitPaymentResponse>(StatusCodes.Status400BadRequest);

            endpoints
                .MapGet(ApiRoutes.GetPaymentDetails, async (
                    [FromServices] IMediator mediator,
                    [FromServices] IHttpContextAccessor httpContextAccessor,
                    [FromBody] PaymentDetailsQuery query,
                    CancellationToken cancellationToken) => await GetDetails(mediator, httpContextAccessor, query, cancellationToken))
                .Produces<PaymentDetailsResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return endpoints;
        }

        static async Task<IResult> Submit(IMediator mediator, IHttpContextAccessor httpContextAccessor, SubmitPaymentDto command, CancellationToken cancellationToken)
        {
            //var merchantReference = httpContextAccessor.GetMerchantReference();
            //command.MerchantReference = merchantReference;

            return Results.Content("nope");

            //var response = await mediator.Send(command, cancellationToken);

            //if (response.PaymentStatus is PaymentStatus.Successful)
            //{
            //    return Results.Accepted($"{ApiRoutes.SubmitPayment}/{response.PaymentReference}", response);
            //}

            //return Results.BadRequest(response);
        }

        static async Task<IResult> GetDetails(IMediator mediator, IHttpContextAccessor httpContextAccessor, PaymentDetailsQuery query, CancellationToken cancellationToken)
        {
            var merchantReference = httpContextAccessor.GetMerchantReference();
            query.MerchantReference = merchantReference;

            return await mediator.Send(query, cancellationToken) is PaymentDetailsResponse details ? Results.Ok(details) : Results.NotFound();

        }
    }
}