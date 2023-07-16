using System.Reflection;
using Application.Common;
using Application.Interfaces;
using Application.Services;
using Application.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceWrapper, ServiceWrapper>();
        services.AddScoped<UserValidator>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthenticationOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthenticationOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthenticationOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
        return services;
    }
}