using WebApi.Middlewares;

namespace WebApi.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this WebApplication app) 
    { 
        app.UseMiddleware<ExceptionHandlingMiddleware>(); 
    }
}