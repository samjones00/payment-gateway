using PaymentGateway.AcquiringBank.CKO;
using PaymentGateway.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Gateway
builder.Host.AddCKOBankConfiguration();

builder.Services.AddGatewayServices(builder.Configuration);
builder.Services.AddGatewayAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJWTAuth();
builder.Services.AddCKOBankServices(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseHealthChecks("/healthcheck"); //test

app.MapControllers();

app.Run();

public partial class Program { }