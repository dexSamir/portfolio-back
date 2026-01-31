using Portfolio.WebAPI.Middlewares;

namespace Portfolio.WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionMiddleware>();
}
