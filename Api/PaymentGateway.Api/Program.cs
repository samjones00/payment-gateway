using MediatR;
using PaymentGateway.Core;
using PaymentGateway.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(IAssemblyMarker));
builder.Services.AddPaymentConfiguration(builder.Configuration, x => { });
builder.Services.AddPaymentValidators();
builder.Services.AddPaymentProcessingServices(builder.Configuration);
builder.Services.AddPaymentDetailServices();

var app = builder.Build();

app.MapEndpoints();

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