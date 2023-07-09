using System.Reflection;
using Application.Interfaces;
using Application.Services;
using Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceWrapper, ServiceWrapper>();
        services.AddScoped<UserValidator>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
}