using PaymentGateway.AcquiringBank.CKO;
using PaymentGateway.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Gateway
builder.Host.AddCKOBankConfiguration();
builder.Services.AddGatewayServices();
builder.Services.AddGatewayAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJWTAuthentication();
builder.Services.AddCKOBankServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }