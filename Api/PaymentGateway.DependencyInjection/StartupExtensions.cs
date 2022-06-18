using System.Text;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentGateway.Core.Providers;
using PaymentGateway.Core.Repositories;
using PaymentGateway.Domain.Configuration;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.DependencyInjection;

public static class StartupExtensions
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Core.IAssemblyMarker).Assembly);
        services.AddMediatR(typeof(Core.IAssemblyMarker));
        services.AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<Domain.IAssemblyMarker>();
            x.DisableDataAnnotationsValidation = true;
        });

        services.AddHttpContextAccessor();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IRepository<Payment>, InMemoryRepository>();
        services.AddMemoryCache();

        //services.AddTransient<IEncryptionProvider, WeakEncryptionProvider>();

        //TODO:
        //Validate config   

        return services;
    }

    public static IServiceCollection AddGatewayAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();
        });

        return services;
    }

    public static IServiceCollection AddSwaggerWithJWTAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Name = "Authorization",
                Description = "Bearer Authentication with JWT Token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
        });

        return services;

    }
}