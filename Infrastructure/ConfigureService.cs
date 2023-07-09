using Application.Interfaces.RepositoryContract.Common;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        return services;
    }

    public static void AddDbConfigure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Project5GDbContext>(options =>
            options.UseNpgsql(connectionString, x=> x.MigrationsAssembly(typeof(Project5GDbContext).Assembly.FullName)));
    }
}