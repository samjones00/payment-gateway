using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api;
using PaymentGateway.Core.Responses;
using PaymentGateway.DependencyInjection;
using PaymentGateway.Domain.Dto;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(ProcessPaymentCommand));
builder.Services.AddPaymentConfiguration(builder.Configuration, x => { });
builder.Services.AddPaymentValidators();
builder.Services.AddPaymentProcessingServices(builder.Configuration);
builder.Services.AddPaymentDetailServices();

var app = builder.Build();

//app.MapEndpoints()

app.MapPost(Routes.ProcessPayment, async ([FromServices] IMediator mediator, ProcessPaymentCommand command) =>
    {
        var response = await mediator.Send(command);

        if (response.PaymentStatus is PaymentStatus.Unsuccessful)
        {
            return Results.BadRequest(response);
        }

        return Results.Created($"{Routes.PaymentDetails}/{response.PaymentReference}", response);
    })
   .Produces<ProcessPaymentResponse>(StatusCodes.Status400BadRequest)
   .Produces<ProcessPaymentResponse>(StatusCodes.Status201Created);

app.MapPost(Routes.PaymentDetails, async ([FromServices] IMediator mediator, PaymentDetailsQuery query) =>
         await mediator.Send(query) is PaymentDetailsResponse details ? Results.Ok(details) : Results.NotFound())
   .Produces<PaymentDetailsResponse>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }